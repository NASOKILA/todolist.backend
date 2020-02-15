using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.ViewModels
{
    public class LoginViewModelTests : BaseModelsTests
    {
        [Fact]
        public void LoginViewModel_Test_Returns_No_Errors()
        {
            //Arrange
            Backend.Models.ViewModels.LoginViewModel LoginViewModel = new Backend.Models.ViewModels.LoginViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                Password = "LELEmale123123%%%"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(LoginViewModel);

            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void LoginViewModel_Test_Returns_Email_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.LoginViewModel LoginViewModel = new Backend.Models.ViewModels.LoginViewModel()
            {
                Password = "LELEmale123123%%%"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(LoginViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("Email is required", result);
        }

        [Fact]
        public void LoginViewModel_Test_Returns_Password_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.LoginViewModel LoginViewModel = new Backend.Models.ViewModels.LoginViewModel()
            {
                Email = "atanasskambitovv@gmail.com"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(LoginViewModel);
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
