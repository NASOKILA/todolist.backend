using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Backend.Controllers;
using TodoList.Backend.Data;
using TodoList.Backend.Models;
using TodoList.Backend.Models.Database;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Models.ViewModels;
using TodoList.Backend.Utils.Repositories;
using TodoList.Backend.Utils.Services;
using Xunit;

namespace TodoList.Backend.UnitTests.Controllers
{
    public class ItemsControllerTests
    {
        private ItemsController _itemsController;
        private ItemRepository _itemRepository;
        private ListRepository _listRepository;
        private UserRepository _userRepository;
        private TodoListDbContext _db;
        private User _user;
        private Backend.Models.Database.TodoList _list;
        private TodoItem _item;
        private AuthController _authController;
        private string _uniqueToken;

        public ItemsControllerTests()
        {
            var mockAuthService = new Mock<IAuthService>();

            var mockAuthServiceGetAuthDataHandle = new Mock<AuthData>();

            var mockLogger = new Mock<ILoggerService>();

            mockAuthService
               .Setup(o => o.HashPassword(It.IsAny<string>()))
               .Returns("AQAAAAEAACcQAAAAENt3qvnso/FOo/LM46ZXTSvnBKcVDAaWlO+YHGcyGnwGz2+wCVAoAZUASC5ZhD6ISw==");

            mockAuthService
               .Setup(o => o.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(true);

            mockAuthService
               .Setup(o => o.GetAuthData(It.IsAny<string>()))
               .Returns(mockAuthServiceGetAuthDataHandle.Object);

            DbContextOptions<TodoListDbContext> options = new DbContextOptionsBuilder<TodoListDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _db = new TodoListDbContext(options);

            _itemRepository = new ItemRepository(_db);

            _userRepository = new UserRepository(mockAuthService.Object, _db);

            _listRepository = new ListRepository(_db);

            var _authService = new AuthService("bRhYJRlZvBj2vW4MrV5HVdPgIE6VMtCFB0kTtJ1m", 2592000);

            _uniqueToken = Guid.NewGuid().ToString();

            _itemsController = new ItemsController(mockLogger.Object, _itemRepository, _listRepository);

            _authController = new AuthController(mockAuthService.Object, _userRepository, mockLogger.Object);

            AddItemToDatabase();
        }

        private void AddItemToDatabase()
        {
            string email = "atanasskambitovv@gmail.com";

            string password = "LELEmale123123%%%";

            _user = new User()
            {
                Email = email,
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = _uniqueToken,
                Lists = new List<Backend.Models.Database.TodoList>(),
                Password = password
            };

            _list = new Backend.Models.Database.TodoList()
            {
                Id = 1,
                Title = "ListOne",
                Items = new List<TodoItem>(),
                UserId = 1
            };

            _item = new TodoItem()
            {
                Id = 1,
                Description = "Item One",
                IsDone = true,
                IsShared = true,
                ListId = 1
            };

            _db.Users.Add(_user);

            _db.Lists.Add(_list);

            _db.Items.Add(_item);

            _db.SaveChanges();
        }

        [Fact]
        public async Task Get_Item_By_Id_Returns_Ok()
        {
            //Arrange

            //Act
            int listsCount = _itemRepository.Count();
            var iactionResult = await _itemsController.Get(_list.Id);
            var objectResponse = iactionResult as ObjectResult;
            var resultObject = objectResponse.Value as TodoItem;

            //Assert
            Assert.Equal(resultObject.Id, _item.Id);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public async Task Get_Shared_Items_Returns_Ok()
        {
            //Arrange
            int sharedItemsFromDb = _db.Items
                .Where(i => i.IsShared == true)
                .ToList()
                .Count();
            
            //Act
            int listsCount = _itemRepository.Count();
            var iactionResult = await _itemsController.Get();
            var objectResponse = iactionResult as ObjectResult;
            var resultObject = objectResponse.Value as List<TodoItem>;
            
            //Assert
            Assert.Equal(resultObject.Count, sharedItemsFromDb);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
        }


        [Fact]
        public async Task Post_Create_Item_By_Id_Returns_Ok()
        {
            //Arrange
            ReceiveItemViewModel receiveItemViewModel = new ReceiveItemViewModel()
            {
                Description = "New Item added",
                IsShared = true,
                IsDone = true
            };

            _db.Items.Remove(_item);

            //Act
            var iactionResult = await _itemsController.Post(_list.Id, receiveItemViewModel) as ObjectResult;
            int listsCount = _itemRepository.Count();

            //Assert
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, iactionResult.StatusCode);
            Assert.Equal($"New Item was created for list {_list.Id}.", iactionResult.Value);
        }

        [Fact]
        public async Task Post_Create_Item_By_Id_Returns_Bad_Request_List_Not_Found()
        {
            //Arrange
            ReceiveItemViewModel receiveItemViewModel = new ReceiveItemViewModel()
            {
                Description = "New Item added",
                IsShared = true,
                IsDone = true
            };

            _db.Items.Remove(_item);

            int wrongId = 9999;

            //Act
            var iactionResult = await _itemsController.Post(wrongId, receiveItemViewModel) as ObjectResult;

            int listsCount = _itemRepository.Count();

            //Assert
            Assert.IsType<BadRequestObjectResult>(iactionResult);
            Assert.Equal(400, iactionResult.StatusCode);
            Assert.Equal($"List does not exist.", iactionResult.Value);
        }

        [Fact]
        public async Task Post_Create_Item_By_Id_Returns_Bad_Request_Invalid_Model_State()
        {
            //Arrange
            ReceiveItemViewModel receiveItemViewModel = new ReceiveItemViewModel()
            {
                Description = "New Item added",
                IsShared = true,
                IsDone = true
            };

            _db.Items.Remove(_item);
            
            //Act
            _itemsController.ModelState.AddModelError("Description", "Description is required.");
            var iactionResult = await _itemsController.Post(_list.Id, receiveItemViewModel);

            //Assert
            Assert.IsType<BadRequestResult>(iactionResult);
        }

        [Fact]
        public async Task Post_Share_Toggle_Item_By_Id_Returns_Bad_Request_Item_Not_Found()
        {
            //Arrange
            ReceiveItemViewModel receiveItemViewModel = new ReceiveItemViewModel()
            {
                Description = "Item",
                IsShared = false,
                IsDone = false
            };

            int wrongId = 9999;

            //Act
            var iactionResult = await _itemsController.Put(wrongId) as ObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(iactionResult);
            Assert.Equal(400, iactionResult.StatusCode);
            Assert.Equal($"Item does not exist.", iactionResult.Value);
        }

        [Fact]
        public async Task Put_Share_Toggle_Item_By_Id_Returns_Bad_Request()
        {
            //Arrange
            ReceiveItemViewModel receiveItemViewModel = new ReceiveItemViewModel()
            {
                Description = "Item",
                IsShared = false,
                IsDone = false
            };

            int wrongId = 9999;

            //Act
            var iactionResult = await _itemsController.Put(wrongId) as ObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(iactionResult);
            Assert.Equal(400, iactionResult.StatusCode);
            Assert.Equal($"Item does not exist.", iactionResult.Value);
        }

        [Fact]
        public async Task Put_Complete_Toggle_Item_By_Id_Returns_Bad_Request_Item_Not_Found()
        {
            //Arrange
            ReceiveItemViewModel receiveItemViewModel = new ReceiveItemViewModel()
            {
                Description = "Item",
                IsShared = false,
                IsDone = false
            };

            int wrongId = 9999;

            //Act
            var iactionResult = await _itemsController.Complete(wrongId) as ObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(iactionResult);
            Assert.Equal(400, iactionResult.StatusCode);
            Assert.Equal($"Item does not exist.", iactionResult.Value);
        }


    }
}
