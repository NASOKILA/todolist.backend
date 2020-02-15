using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.ViewModels
{
    public class TodoItemViewModelTests : BaseModelsTests
    {
        [Fact]
        public void TodoItemViewModel_Test_Returns_No_Errors()
        {
            //Arrange
            Backend.Models.ViewModels.TodoItemViewModel TodoItemViewModel = new Backend.Models.ViewModels.TodoItemViewModel()
            {
                Description = "Item One",
                IsDone = true,
                IsShared = true,
                Id = 1
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(TodoItemViewModel);

            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void TodoItemViewModel_Test_Returns_Description_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.TodoItemViewModel TodoItemViewModel = new Backend.Models.ViewModels.TodoItemViewModel()
            {
                Id = 1,   
                IsDone = true,
                IsShared = true
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(TodoItemViewModel);
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
