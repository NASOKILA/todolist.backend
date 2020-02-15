using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.ViewModels
{
    public class UserEditViewModelTests : BaseModelsTests
    {
        [Fact]
        public void UserEditViewModel_Test_Returns_No_Errors()
        {
            //Arrange
            Backend.Models.ViewModels.UserEditViewModel userEditViewModel = new Backend.Models.ViewModels.UserEditViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas",
                LastName = "Kambitov"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(userEditViewModel);
            
            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void UserEditViewModel_Test_Returns_Email_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.UserEditViewModel userEditViewModel = new Backend.Models.ViewModels.UserEditViewModel()
            {
                FirstName = "Atanas",
                LastName = "Kambitov"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(userEditViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("Email is required", result);
        }

        [Fact]
        public void UserEditViewModel_Test_Returns_FirstName_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.UserEditViewModel userEditViewModel = new Backend.Models.ViewModels.UserEditViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                LastName = "Kambitov"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(userEditViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("FirstName is required", result);
        }

        [Fact]
        public void UserEditViewModel_Test_Returns_LastName_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.UserEditViewModel userEditViewModel = new Backend.Models.ViewModels.UserEditViewModel()
            {
                Email = "atanasskambitovv@gmail.com",
                FirstName = "Atanas"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(userEditViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("LastName is required", result);
        }

        protected override List<ValidationResult> ValidateModel(object model)
        {
            return base.ValidateModel(model);
        }
    }
}
