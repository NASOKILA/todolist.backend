using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TodoList.Backend.Models.Database;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.Database
{
    public class UserModelTests : BaseModelsTests
    {
        [Fact]
        public void User_Model_Test_Returns_No_Errors()
        {
            //Arrange
            User user = new User()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.Database.TodoList>(),
                Password = "LELEmale123123%%%"
            };
            
            //Act
            var lstErrors = ValidateModel(user);


            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void User_Model_Test_Returns_Unique_Token_Is_Required()
        {
            //Arrange
            User user = new User()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                Lists = new List<Backend.Models.Database.TodoList>(),
                Password = "LELEmale123123%%%"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(user);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("UniqueToken is required", result);
        }

        [Fact]
        public void User_Model_Test_Returns_First_Name_Is_Required()
        {
            //Arrange
            User user = new User()
            {
                Email = "atanasskambitovv@gmail.com",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.Database.TodoList>(),
                Password = "LELEmale123123%%%"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(user);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("FirstName is required", result);
        }

        [Fact]
        public void User_Model_Test_Returns_Last_Name_Is_Required()
        {
            //Arrange
            User user = new User()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.Database.TodoList>(),
                Password = "LELEmale123123%%%"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(user);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("LastName is required", result);
        }

        [Fact]
        public void User_Model_Test_Returns_Is_Email_Is_Required()
        {
            //Arrange
            User user = new User()
            {
                FirstName = "Atanas",
                LastName = "Kambitov",
                IsAdmin = true,
                Id = 1,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.Database.TodoList>(),
                Password = "LELEmale123123%%%"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(user);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("Email is required", result);
        }

        [Fact]
        public void User_Model_Test_Returns_Is_Password_Is_Required()
        {
            //Arrange
            User user = new User()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                IsAdmin = true,
                Id = 1,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.Database.TodoList>()
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(user);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("Password is required", result);
        }

        protected override List<ValidationResult> ValidateModel(object model)
        {
            return base.ValidateModel(model);
        }
    }
}
