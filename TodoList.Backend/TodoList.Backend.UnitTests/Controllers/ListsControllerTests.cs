using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
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
    public class ListsControllerTests
    {
        private ListsController _listsController;
        private ListRepository _listRepository;
        private UserRepository _userRepository;
        private TodoListDbContext _db;
        private User _user;
        private Backend.Models.Database.TodoList _list;
        private AuthController _authController;
        private string _uniqueToken;

        public ListsControllerTests()
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
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            _db = new TodoListDbContext(options);

            _listRepository = new ListRepository(_db);
            
            _userRepository = new UserRepository(mockAuthService.Object, _db);

            var _authService = new AuthService("bRhYJRlZvBj2vW4MrV5HVdPgIE6VMtCFB0kTtJ1m", 2592000);

            _uniqueToken = Guid.NewGuid().ToString();
            
            _listsController = new ListsController(mockLogger.Object, _listRepository, _userRepository);
            
            _authController = new AuthController(mockAuthService.Object, _userRepository, mockLogger.Object);

            AddListToDatabase();
        }
        
        private void AddListToDatabase()
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

            _db.Users.Add(_user);

            _db.Lists.Add(_list);

            _db.SaveChanges();
        }
        
        [Fact]
        public async Task Get_My_Lists_Returns_Ok()
        {
            //Arrange
            
            //Act
            int listsCount = _listRepository.Count();
            var iactionResult = await _listsController.Get(_uniqueToken);
            var objectResponse = iactionResult as ObjectResult;
            var resultObject = objectResponse.Value as List<Backend.Models.Database.TodoList>;

            //Assert
            Assert.Equal(resultObject.Count, listsCount);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public async Task Get_List_By_Id_Returns_Ok()
        {
            //Arrange
            
            //Act
            int listsCount = _listRepository.Count();
            var iactionResult = await _listsController.Get(_user.Id);
            var objectResponse = iactionResult as ObjectResult;
            var resultObject = objectResponse.Value as Backend.Models.Database.TodoList;

            //Assert
            Assert.Equal(resultObject.Id, _list.Id);
            Assert.Equal(resultObject.Title, _list.Title);
            Assert.Equal(resultObject.UserId, _list.UserId);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public async Task Get_List_By_Id_Returns_Bad_Request_User_Not_Found()
        {
            //Arrange
            int wrongId = -1;

            //Act
            int listsCount = _listRepository.Count();
            var iactionResult = await _listsController.Get(wrongId) as ObjectResult;
            var resultObject = iactionResult.Value as Backend.Models.Database.TodoList;

            //Assert
            Assert.IsType<BadRequestObjectResult>(iactionResult);
            Assert.Equal(400, iactionResult.StatusCode);
            Assert.Equal("List does not exist.", iactionResult.Value);
        }

        [Fact]
        public async Task Post_Create_List_By_Id_Returns_Ok()
        {
            //Arrange
            ListReceivedViewModel listReceivedViewModel = new ListReceivedViewModel()
            {
                Title="List Two"
            };

            _db.Lists.Remove(_list);

            //Act
            var iactionResult = await _listsController.Post(_user.UniqueToken, listReceivedViewModel) as ObjectResult;
            int listsCount = _listRepository.Count();
            
            //Assert
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, iactionResult.StatusCode);
            Assert.Equal("New list was created.", iactionResult.Value);
        }

        [Fact]
        public async Task Post_Create_List_By_Id_Returns_Bad_Request_User_Not_Found()
        {
            //Arrange
            ListReceivedViewModel listReceivedViewModel = new ListReceivedViewModel()
            {
                Title = "List Two"
            };
            
            //Act
            var iactionResult = await _listsController.Post(Guid.NewGuid().ToString(), listReceivedViewModel) as ObjectResult;
           
            //Assert
            Assert.IsType<BadRequestObjectResult>(iactionResult);
            Assert.Equal(400, iactionResult.StatusCode);
            Assert.Equal("User does not exist.", iactionResult.Value);
        }

        [Fact]
        public async Task Post_Create_List_By_Id_Returns_Bad_Request_Invalid_Model_State()
        {
            //Arrange
            ListReceivedViewModel listReceivedViewModel = new ListReceivedViewModel()
            {
                Title = "List Two"
            };

            _db.Lists.Remove(_list);

            _listsController.ModelState.AddModelError("Title", "Title is required.");

            //Act
            var iactionResult = await _listsController.Post(Guid.NewGuid().ToString(), listReceivedViewModel);

            //Assert
            Assert.IsType<BadRequestResult>(iactionResult);
        }

        [Fact]
        public async Task Put_Update_List_Returns_Ok()
        {
            //Arrange
            ListReceivedViewModel listReceivedViewModel = new ListReceivedViewModel()
            {
                Title = "List Two (Updated)"
            };
            
            //Act
            var iactionResult = await _listsController.Put(_list.Id, listReceivedViewModel);
            var objectResult = iactionResult as ObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal($"List {listReceivedViewModel.Title} was updated.", objectResult.Value);
        }

        [Fact]
        public async Task Put_Update_List_Returns_Bad_Request_LIst_Does_Not_Exist()
        {
            //Arrange
            ListReceivedViewModel listReceivedViewModel = new ListReceivedViewModel()
            {
                Title = "List Two (Updated)"
            };

            int wrongId = 9999;

            //Act
            var iactionResult = await _listsController.Put(wrongId, listReceivedViewModel);
            var objectResult = iactionResult as ObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(iactionResult);
            Assert.Equal(400, objectResult.StatusCode);
            Assert.Equal($"List does not exist.", objectResult.Value);
        }
        
        [Fact]
        public async Task Delete_List_By_Id_Returns_Ok()
        {
            //Arrange

            //Act
            int listsCount = _listRepository.Count();
            var iactionResult = await _listsController.Delete(_list.Id) as ObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, iactionResult.StatusCode);
            Assert.Equal($"List {_list.Title} was deleted.", iactionResult.Value);
        }

        [Fact]
        public async Task Delete_List_By_Id_Returns_Bad_Request_List_Does_Not_Exist()
        {
            //Arrange

            //Act
            int listsCount = _listRepository.Count();
            var iactionResult = await _listsController.Delete(_list.Id) as ObjectResult;
            int listsCountAfterDelete = _listRepository.Count();

            //Assert
            Assert.Equal(listsCount, listsCountAfterDelete);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, iactionResult.StatusCode);
            Assert.Equal($"List {_list.Title} was deleted.", iactionResult.Value);
        }
    }
}