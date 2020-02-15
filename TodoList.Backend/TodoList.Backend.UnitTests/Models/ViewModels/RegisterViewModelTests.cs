using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.ViewModels
{
    public class RegisterViewModelTests : BaseModelsTests
    {
        [Fact]
        public void RegisterViewModel_Test_Returns_No_Errors()
        {
            //Arrange
            Backend.Models.ViewModels.RegisterViewModel registerViewModel = new Backend.Models.ViewModels.RegisterViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                Password = "LELEmale12313%%%",
                IsAdmin = true,
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(registerViewModel);

            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void RegisterViewModel_Test_Returns_Email_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.RegisterViewModel registerViewModel = new Backend.Models.ViewModels.RegisterViewModel()
            {
                FirstName = "Atanas",
                LastName = "Kambitov",
                Password = "LELEmale12313%%%",
                IsAdmin = true,
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(registerViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("Email is required", result);
        }

        [Fact]
        public void RegisterViewModel_Test_Returns_FirstName_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.RegisterViewModel registerViewModel = new Backend.Models.ViewModels.RegisterViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                LastName = "Kambitov",
                Password = "LELEmale12313%%%",
                IsAdmin = true,
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(registerViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("FirstName is required", result);
        }

        [Fact]
        public void RegisterViewModel_Test_Returns_LastName_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.RegisterViewModel registerViewModel = new Backend.Models.ViewModels.RegisterViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                Password = "LELEmale12313%%%",
                IsAdmin = true,
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(registerViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("LastName is required", result);
        }
        
        [Fact]
        public void RegisterViewModel_Test_Returns_Password_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.RegisterViewModel registerViewModel = new Backend.Models.ViewModels.RegisterViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov",
                IsAdmin = true,
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(registerViewModel);
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
