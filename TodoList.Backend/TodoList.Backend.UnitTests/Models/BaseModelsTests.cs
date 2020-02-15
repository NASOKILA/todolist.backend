using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Backend.UnitTests.Models
{
    public abstract class BaseModelsTests
    {
        protected virtual List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
