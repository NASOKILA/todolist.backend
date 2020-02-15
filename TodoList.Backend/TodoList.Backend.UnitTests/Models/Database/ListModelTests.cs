using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.Database
{
    public class ListModelTests : BaseModelsTests
    {
        [Fact]
        public void List_Model_Test_Returns_No_Errors()
        {
            //Arrange
            Backend.Models.Database.TodoList list = new Backend.Models.Database.TodoList()
            {
                Id = 1,
                Items = new List<Backend.Models.Database.TodoItem>(),
                Title = "List One"
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(list);

            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void List_Model_Test_Returns_Title_Is_Required()
        {
            //Arrange
            Backend.Models.Database.TodoList list = new Backend.Models.Database.TodoList()
            {
                Id = 1,
                Items = new List<Backend.Models.Database.TodoItem>()
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(list);
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
