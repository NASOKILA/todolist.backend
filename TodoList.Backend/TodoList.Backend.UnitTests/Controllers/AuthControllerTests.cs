using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Backend.Controllers;
using TodoList.Backend.Data;
using TodoList.Backend.Models;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Models.ViewModels;
using TodoList.Backend.Utils.Repositories;
using Xunit;

namespace TodoList.Backend.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        AuthController _authController;

        UserRepository _userRepository;
        
        public AuthControllerTests()
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
            
             var db = new TodoListDbContext(options);
            
            _userRepository = new UserRepository(mockAuthService.Object, db);
           
            _authController = new AuthController(mockAuthService.Object, _userRepository, mockLogger.Object);
        }
        
        [Fact]
        public async Task Post_Login_Returns_Ok()
        {
            //Arrange
            string email = "atanasskambitovv@gmail.com";
            string password = "LELEmale123123%%%";
            
            UserViewModel user = new UserViewModel()
            {
                Email = email,
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<TodoListViewModel>(),
            };
            
            await _userRepository.AddAsync(user, password);
            _userRepository.CommitChanges();
            
            LoginViewModel model = new LoginViewModel
            {
                Email = email,
                Password = password
            };

            //Act
            int usersCount = _userRepository.Count();
            var result = await _authController.Post(model);
            var objectResponse = result as ObjectResult;

            //Assert
            Assert.Equal(1, usersCount);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResponse.StatusCode);
        }
        
        [Fact]
        public async Task Post_Login_Returns_Bad_Request_No_user_With_This_Email()
        {
            //Arrange
            string password = "LELEmale123123%%%";

            UserViewModel user = new UserViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<TodoListViewModel>(),
            };

            await _userRepository.AddAsync(user, password);
            _userRepository.CommitChanges();
            
            LoginViewModel model = new LoginViewModel
            {
                Email = "atanasskambitovv2222@gmail.com",
                Password = password
            };

            //Act
            var result = await _authController.Post(model);
            var usersCount = _userRepository.Count();
            var objectResponse = result as ObjectResult;
            var errorMessage = objectResponse.Value.ToString();

            //Assert
            Assert.Equal(1, usersCount);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResponse.StatusCode);
            Assert.Equal("No user with this email", errorMessage);
        }
        
        [Fact]
        public async Task Post_Login_Returns_Bad_Request_Invalid_Password()
        {
            var mockAuthService = new Mock<IAuthService>();

            var mockAuthServiceGetAuthDataHandle = new Mock<AuthData>();
            
            var mockLogger = new Mock<ILoggerService>();

            mockAuthService
               .Setup(o => o.HashPassword(It.IsAny<string>()))
               .Returns("AQAAAAEAACcQAAAAENt3qvnso/FOo/LM46ZXTSvnBKcVDAaWlO+YHGcyGnwGz2+wCVAoAZUASC5ZhD6ISw==");

            mockAuthService
               .Setup(o => o.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(false);

            mockAuthService
               .Setup(o => o.GetAuthData(It.IsAny<string>()))
               .Returns(mockAuthServiceGetAuthDataHandle.Object);

            DbContextOptions<TodoListDbContext> options = new DbContextOptionsBuilder<TodoListDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new TodoListDbContext(options);

            _userRepository = new UserRepository(mockAuthService.Object, db);

            var authController = new AuthController(mockAuthService.Object, _userRepository, mockLogger.Object);
            
            //Arrange
            string email = "atanasskambitovv@gmail.com";
            UserViewModel user = new UserViewModel()
            {
                Email = email,
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<TodoListViewModel>(),
            };

            await _userRepository.AddAsync(user, "LELEmale123123%%%");
            _userRepository.CommitChanges();
            
            LoginViewModel model = new LoginViewModel
            {
                Email = email,
                Password = "Wrong Password"
            };

            //Act
            var result = await authController.Post(model);
            var usersCount = _userRepository.Count();
            var objectResponse = result as ObjectResult;
            var errorMessage = objectResponse.Value.ToString();

            //Assert
            Assert.Equal(1, usersCount);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResponse.StatusCode);
            Assert.Equal("Invalid password", errorMessage);
        }
        
        [Fact]
        public async Task Post_Login_Returns_Bad_Request_Invalid_Model_State_Missing_Password()
        {
            //Arrange
            string password = "LELEmale123123%%%";

            UserViewModel user = new UserViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<TodoListViewModel>(),
            };

            await _userRepository.AddAsync(user, password);
            _userRepository.CommitChanges();
            
            LoginViewModel model = new LoginViewModel
            {
                Email = "atanasskambitovv@gmail.com"
            };


            _authController.ModelState.AddModelError("Password", "Password is required.");

            //Act
            var result = await _authController.Post(model);
            var usersCount = _userRepository.Count();
            var objectResponse = result as ObjectResult;
            var errorMessage = objectResponse.Value.ToString();

            //Assert
            Assert.Equal(1, usersCount);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResponse.StatusCode);
        }
        
        [Fact]
        public async Task Post_Login_Returns_Bad_Request_Invalid_Model_State_Missing_Email()
        {
            //Arrange
            string password = "LELEmale123123%%%";

            UserViewModel user = new UserViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<TodoListViewModel>(),
            };

            await _userRepository.AddAsync(user, password);
            _userRepository.CommitChanges();

            LoginViewModel model = new LoginViewModel
            {
                Email = null,
                Password = password
            };


            _authController.ModelState.AddModelError("Email", "Email is required.");

            //Act
            var result = await _authController.Post(model);
            var usersCount = _userRepository.Count();
            var objectResponse = result as ObjectResult;

            //Assert
            Assert.Equal(1, usersCount);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResponse.StatusCode);
        }
        
        [Fact]
        public async Task Post_Register_Returns_Ok()
        {
            //Arrange
            string email = "atanasskambitovv@gmail.com";
            string password = "LELEmale123123%%%";
            
            RegisterViewModel model = new RegisterViewModel
            {
                Email = email,
                Password = password,
                FirstName = "Atanas",
                LastName = "Kambitov",
                IsAdmin = true,
            };

            //Act
            var result = await _authController.Post(model);
            int usersCount = _userRepository.Count();
            var objectResponse = result as ObjectResult;

            //Assert
            Assert.Equal(1, usersCount);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResponse.StatusCode);
        }
        
        [Fact]
        public async Task Post_Register_Returns_Bad_Request_user_Already_Exists()
        {
            //Arrange
            string email = "atanasskambitovv@gmail.com";
            string password = "LELEmale123123%%%";
            
            UserViewModel user = new UserViewModel()
            {
                Email = email,
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<TodoListViewModel>(),
            };

            await _userRepository.AddAsync(user, password);
            _userRepository.CommitChanges();


            RegisterViewModel model = new RegisterViewModel
            {
                Email = email,
                Password = password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin,
            };

            //Act
            var result = await _authController.Post(model);
            var usersCount = _userRepository.Count();
            var objectResponse = result as ObjectResult;
            var errorMessage = objectResponse.Value.ToString();

            //Assert
            Assert.Equal(1, usersCount);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResponse.StatusCode);
            Assert.Equal("User with this email already exists", errorMessage);
        }
        
        [Fact]
        public async Task Post_Register_Returns_Bad_Request_Invalid_Model_State()
        {
            //Arrange
            RegisterViewModel model = new RegisterViewModel
            {
                Email = "atanasskambitovv@gmail.com"
            };
            
            _authController.ModelState.AddModelError("Password", "Password is required." );

            //Act
            var result = await _authController.Post(model);
            int usersCount = _userRepository.Count();
            var objectResponse = result as ObjectResult;

            //Assert
            Assert.Equal(0, usersCount);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResponse.StatusCode);
        }
    }
}
