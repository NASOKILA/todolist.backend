using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.ViewModels
{
    public class ReceiveItemViewModelTests : BaseModelsTests
    {
        [Fact]
        public void ReceiveItemViewModel_Test_Returns_No_Errors()
        {
            //Arrange
            Backend.Models.ViewModels.ReceiveItemViewModel receiveItemViewModel = new Backend.Models.ViewModels.ReceiveItemViewModel()
            {
                Description = "Item One",
                IsDone = true,
                IsShared = true
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(receiveItemViewModel);

            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void ReceiveItemViewModel_Test_Returns_Description_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.ReceiveItemViewModel receiveItemViewModel = new Backend.Models.ViewModels.ReceiveItemViewModel()
            {
                IsDone = true,
                IsShared = true
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(receiveItemViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("Description is required", result);
        }

        protected override List<ValidationResult> ValidateModel(object model)
        {
            return base.ValidateModel(model);
        }
    }
}
