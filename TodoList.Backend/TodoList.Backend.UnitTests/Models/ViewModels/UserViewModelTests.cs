using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.ViewModels
{
    public class UserViewModelTests : BaseModelsTests
    {
        [Fact]
        public void UserViewModel_Test_Returns_No_Errors()
        {
            //Arrange
            Backend.Models.ViewModels.UserViewModel userViewModel = new Backend.Models.ViewModels.UserViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.ViewModels.TodoListViewModel>()
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(userViewModel);

            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void UserViewModel_Test_Returns_Email_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.UserViewModel userViewModel = new Backend.Models.ViewModels.UserViewModel()
            {
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.ViewModels.TodoListViewModel>()
            };
            
            //Act
            List<ValidationResult> lstErrors = ValidateModel(userViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("Email is required", result);
        }

        [Fact]
        public void UserViewModel_Test_Returns_FirstName_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.UserViewModel userViewModel = new Backend.Models.ViewModels.UserViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.ViewModels.TodoListViewModel>()
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(userViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("FirstName is required", result);
        }

        [Fact]
        public void UserViewModel_Test_Returns_LastName_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.UserViewModel userViewModel = new Backend.Models.ViewModels.UserViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                Id = 1,
                IsAdmin = true,
                UniqueToken = Guid.NewGuid().ToString(),
                Lists = new List<Backend.Models.ViewModels.TodoListViewModel>()
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(userViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("LastName is required", result);
        }

        [Fact]
        public void UserViewModel_Test_Returns_UniqueToken_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.UserViewModel userViewModel = new Backend.Models.ViewModels.UserViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                Id = 1,
                IsAdmin = true,
                Lists = new List<Backend.Models.ViewModels.TodoListViewModel>()
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(userViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("UniqueToken is required", result);
        }

        [Fact]
        public void UserViewModel_Test_Returns_UniqueToken_Cannot_Be_Longer_Than_60_Characters()
        {
            //Arrange
            Backend.Models.ViewModels.UserViewModel userViewModel = new Backend.Models.ViewModels.UserViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                UniqueToken = "123456789101234567891012345678910ajdiauwjhuhr782h3wif4387g43h8g7h34g7893h4398ghfawfawfwgagawfwgaw3og",
                Id = 1,
                IsAdmin = true,
                Lists = new List<Backend.Models.ViewModels.TodoListViewModel>()
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(userViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("UniqueToken can't be longer than 60 characters", result);
        }

        protected override List<ValidationResult> ValidateModel(object model)
        {
            return base.ValidateModel(model);
        }
    }
}
