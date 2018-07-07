using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Refactored.Controllers;
using Refactored.Logic;

namespace Refactored.Utilities
{
	public static class ModelStateUtilities
    {
	    private static List<InvalidField> GetInvalidFields(ModelStateDictionary modelState)
	    {
		    var invalidfeiFields = new List<InvalidField>();

		    foreach (var field in modelState)
		    {

			    var errors = field.Value.Errors;

			    if (errors == null || errors.Count <= 0) continue;

			    var errorMessages = errors.Select(
				    error => string.IsNullOrEmpty(error.ErrorMessage)
					    ? "Field Invalid"
					    : error.ErrorMessage).ToList();

			    var invalidField = new InvalidField
			    {
				    FieldName = field.Key,
				    Errors = errorMessages
			    };

			    invalidfeiFields.Add(invalidField);
		    }

		    return invalidfeiFields;
	    }

	    public static IActionResult GetInvalidModelStateResult(ModelStateDictionary modelState)
	    {
		    var invalidFields = GetInvalidFields(modelState);

		    return new BadRequestObjectResult(
			    new ErrorResult
			    {
				    ErrorCode = 4000,
				    Message = "One or more fields have invalid values. See details for more information",
				    Details = invalidFields
			    });
	    }


    }
}
