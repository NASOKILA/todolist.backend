using System;
using TodoList.Backend.Models;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Utils.Services;
using System.Security.Claims;
using System.Text;
using Xunit;
using TodoList.Backend.Data;
using Microsoft.EntityFrameworkCore;
using TodoList.Backend.Models.Database;
using System.Collections.Generic;

namespace TodoList.Backend.UnitTests.Services
{
    public class AuthServiceTests
    {
        private IAuthService _authService;

        private TodoListDbContext _db;

        private User _user;

        public AuthServiceTests()
        {
            _authService = new AuthService("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjYyM2VhMjM5LTljODItNDQ1YS1hOTAzLTA1YjdlNmVjZTlhNSIsIm5iZiI6MTU4MTYyMjk5NywiZXhwIjoxNTg0MjE0OTk3LCJpYXQiOjE1ODE2MjI5OTd9.Ik0-1dCwFBYyU4Gi8_PAfzJMgFsorCZs6keXOtVMKeY", 1584214997);

            DbContextOptions<TodoListDbContext> options = new DbContextOptionsBuilder<TodoListDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _db = new TodoListDbContext(options);

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

        //[Fact]
        //public void Get_Auth_Data_Returns_Ok()
        //{
        //    //Arrange
        //    string uniqueToken = _user.UniqueToken;

        //    //Act
        //    var authData = _authService.GetAuthData(uniqueToken);
            
        //    //Assert
        //    Assert.IsType<AuthData>(authData);
        //    Assert.IsType<string>(authData.Token);
        //    Assert.IsType<long>(authData.TokenExpirationTime);
        //    Assert.IsType<string>(authData.UniqueToken);
        //}

        [Fact]
        public void Get_Auth_Data_Throws_File_Not_Found_Exception()
        {
            //Arrange
            string uniqueToken = _user.UniqueToken;
            
            //Act

            //Assert
            Assert.Throws<System.IO.FileNotFoundException>(() => _authService.GetAuthData(uniqueToken));
        }
        
        [Fact]
        public void Get_Hashed_Password_Returns_String()
        {
            //Arrange
            string userPassowrd = _user.Password;
            
            //Act
            var hashedPassword = _authService.HashPassword(userPassowrd);

            //Assert
            Assert.IsType<string>(hashedPassword);
            Assert.Equal(84, hashedPassword.Length);
        }

        [Fact]
        public void Verify_Password__Returns_Boolean_True()
        {
            //Arrange
            string actualPassowrd = _user.Password;
            string hashedPassword = _authService.HashPassword(actualPassowrd);

            //Act
            var passwordMatches = _authService.VerifyPassword(actualPassowrd, hashedPassword); 

            //Assert
            Assert.Equal(84, hashedPassword.Length);
            Assert.IsType<bool>(passwordMatches);
            Assert.True(passwordMatches);
        }
    }
}
