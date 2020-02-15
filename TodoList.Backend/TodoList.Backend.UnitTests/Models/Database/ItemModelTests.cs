using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;

namespace TodoList.Backend.UnitTests.Models.Database
{
    public class ItemModelTests : BaseModelsTests
    {
        [Fact]
        public void Item_Model_Test_Returns_No_Errors()
        {
            //Arrange
            Backend.Models.Database.TodoItem item = new Backend.Models.Database.TodoItem()
            {
                Id = 1,
                Description = "Item 1",
                IsDone = true,
                IsShared = true
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(item);

            //Assert
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public void Item_Model_Test_Returns_Description_Is_Required()
        {
            //Arrange
            Backend.Models.Database.TodoItem item = new Backend.Models.Database.TodoItem()
            {
                Id = 1,
                IsDone = true,
                IsShared = true
            };

            //Act
            List<ValidationResult> lstErrors = ValidateModel(item);
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
