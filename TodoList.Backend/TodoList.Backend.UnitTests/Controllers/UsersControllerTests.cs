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
using Xunit;

namespace TodoList.Backend.UnitTests.Controllers
{
    public class UsersControllerTests
    {
        private UsersController _usersController;

        private UserRepository _userRepository;

        private TodoListDbContext _db;

        private User _user;

        public UsersControllerTests()
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

            _userRepository = new UserRepository(mockAuthService.Object, _db);

            _usersController = new UsersController(mockLogger.Object, _userRepository);

            AddUserToDataBase();
        }

        private void AddUserToDataBase()
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
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.Database.TodoList>(),
                Password = password
            };

            _db.Users.Add(_user);
            _db.SaveChanges();
        }

        [Fact]
        public async Task Get_All_users_Returns_Ok()
        {
            //Arrange

            //Act
            int usersCount = _userRepository.Count();
            var iactionResult = await _usersController.Get();
            var objectResponse = iactionResult as ObjectResult;
            var resultObject = objectResponse.Value as List<UserViewModel>;

            //Assert
            Assert.Equal(resultObject.Count, usersCount);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public async Task Get_user_By_Id_Returns_Ok()
        {
            //Arrange


            //Act
            int usersCount = _userRepository.Count();
            var iactionResult = await _usersController.Get(_user.Id) as ObjectResult;
            var objectResponse = iactionResult as ObjectResult;
            var userEditViewModel = objectResponse.Value as UserEditViewModel;

            //Assert
            Assert.Equal(_user.Email, userEditViewModel.Email);
            Assert.Equal(_user.FirstName, userEditViewModel.FirstName);
            Assert.Equal(_user.LastName, userEditViewModel.LastName);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public async Task Get_user_By_Id_Returns_Bad_Request_user_Does_Not_Exist()
        {
            //Arrange
            int wrongId = 2;

            //Act
            int usersCount = _userRepository.Count();
            var iactionResult = await _usersController.Get(wrongId) as ObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(iactionResult);
            Assert.Equal(400, iactionResult.StatusCode);
            Assert.Equal("User does not exist.", iactionResult.Value);
        }

        [Fact]
        public async Task Put_Toggle_Admin_Returns_Ok_user()
        {
            //Arrange
            UserAdminViewModel userAdminViewModel = new UserAdminViewModel()
            {
                Id = _user.Id,
                IsAdmin = _user.IsAdmin
            };

            //Act
            var iactionResult = await _usersController.Put(userAdminViewModel) as ObjectResult;
            var objectResponse = iactionResult as ObjectResult;
            string result = objectResponse.Value as string;
            //Assert

            Assert.Equal($"{_user.FirstName} set to User successfully.", result);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public async Task Put_Toggle_Admin_Returns_Ok_Admin()
        {
            //Arrange
            UserAdminViewModel userAdminViewModel = new UserAdminViewModel()
            {
                Id = _user.Id,
                IsAdmin = false
            };

            //Act
            int usersCount = _userRepository.Count();
            var iactionResult = await _usersController.Put(userAdminViewModel);
            var objectResponse = iactionResult as ObjectResult;
            string result = objectResponse.Value as string;

            //Assert
            Assert.Equal($"{_user.FirstName} set to Admin successfully.", result);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public async Task Put_Toggle_Admin_Returns_Bad_Request_Invalis_ModelState()
        {
            //Arrange
            UserAdminViewModel userAdminViewModel = new UserAdminViewModel()
            {
                Id = _user.Id
            };

            _usersController.ModelState.AddModelError("IsAdmin", "IsAdmin is required.");

            //Act
            int usersCount = _userRepository.Count();
            var iactionResult = await _usersController.Put(userAdminViewModel);

            //Assert
            Assert.IsType<BadRequestResult>(iactionResult);
        }

        [Fact]
        public async Task Delete_user_Returns_Ok()
        {
            UserAdminViewModel userAdminViewModel = new UserAdminViewModel()
            {
                Id = _user.Id,
                IsAdmin = false
            };

            //Act
            var iactionResult = await _usersController.Delete(1) as ObjectResult;
            var objectResponse = iactionResult as ObjectResult;
            string result = objectResponse.Value as string;
            int usersCount = _userRepository.Count();

            //Assert
            Assert.Equal($"{_user.FirstName} was deleted successfully.", result);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
            Assert.Equal(0, usersCount);
        }

        [Fact]
        public async Task Put_Update_User_Returns_Ok()
        {
            UserEditViewModel userEditViewModel = new UserEditViewModel()
            {
                Email = "asen_kambitov@gmail.com",
                FirstName = "Asen",
                LastName = "Kambitov"
            };

            //Act
            var iactionResult = await _usersController.Put(_user.Id, userEditViewModel) as ObjectResult;
            var objectResponse = iactionResult as ObjectResult;
            string result = objectResponse.Value as string;

            var updatedUserResult = await _usersController.Get(_user.Id) as ObjectResult;
            var userEditViewModelReceived = updatedUserResult.Value as UserEditViewModel;

            //Assert
            Assert.Equal(userEditViewModelReceived.FirstName, userEditViewModel.FirstName);
            Assert.Equal(userEditViewModelReceived.LastName, userEditViewModel.LastName);
            Assert.Equal(userEditViewModelReceived.Email, userEditViewModel.Email);
            Assert.Equal($"{_user.FirstName} was updated successfully.", result);
            Assert.IsType<OkObjectResult>(iactionResult);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public async Task Put_Update_User_Returns_Bad_Request_User_Does_Not_Exist()
        {
            UserEditViewModel userEditViewModel = new UserEditViewModel()
            {
                Email = "asen_kambitov@gmail.com",
                FirstName = "Asen",
                LastName = "Kambitov"
            };

            int wrongId = 9999;
            //Act
            var iactionResult = await _usersController.Put(wrongId, userEditViewModel) as ObjectResult;
            string result = iactionResult.Value as string;

            //Assert
            Assert.Equal($"User does not exist.", result);
            Assert.IsType<BadRequestObjectResult>(iactionResult);
            Assert.Equal(400, iactionResult.StatusCode);
        }
    }
}
