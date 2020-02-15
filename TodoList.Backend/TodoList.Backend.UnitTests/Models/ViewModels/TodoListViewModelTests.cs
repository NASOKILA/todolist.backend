using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.ViewModels
{
    public class TodoListViewModelTests : BaseModelsTests
    {
        [Fact]
        public void TodoListViewModel_Test_Returns_No_Errors()
        {
            //Arrange
            Backend.Models.ViewModels.TodoListViewModel todoListViewModel = new Backend.Models.ViewModels.TodoListViewModel()
            {
                Title = "List One",
                Id = 1,
                Items = new List<Backend.Models.ViewModels.TodoItemViewModel>()
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(todoListViewModel);
     
            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void TodoListViewModel_Test_Returns_Title_Is_Required()
        {
            //Arrange
            Backend.Models.ViewModels.TodoListViewModel todoListViewModel = new Backend.Models.ViewModels.TodoListViewModel()
            {
                Id = 1,
                Items = new List<Backend.Models.ViewModels.TodoItemViewModel>()
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(todoListViewModel);
            string result = lstErrors[0].ToString();

            //Assert
            Assert.True(lstErrors.Count == 1);
            Assert.Equal("Title is required", result);
        }
        
        protected override List<ValidationResult> ValidateModel(object model)
        {
            return base.ValidateModel(model);
        }
    }
}
