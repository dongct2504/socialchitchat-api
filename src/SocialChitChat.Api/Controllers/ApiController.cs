﻿using FluentResults;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialChitChat.Business.Common;
using SocialChitChat.Business.Common.Errors;

namespace SocialChitChat.Api.Controllers;

[ApiController]
[ServiceFilter(typeof(LogUserActivity))]
public class ApiController : ControllerBase
{
    protected ActionResult Problem(List<IError> errors)
    {
        IError firstError = errors.First();

        switch (firstError)
        {
            case NotFoundError:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: firstError.Message);
            case BadRequestError:
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: firstError.Message);
            case ConflictError:
                return Problem(statusCode: StatusCodes.Status409Conflict, detail: firstError.Message);
            case UnauthorizeError:
                return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: firstError.Message);
            default:
                return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: firstError.Message);
        }
    }

    protected ActionResult Problem(List<ValidationFailure> errors)
    {
        ModelStateDictionary keyValuePairs = new ModelStateDictionary();

        foreach (ValidationFailure failure in errors)
        {
            keyValuePairs.AddModelError(failure.PropertyName, failure.ErrorMessage);
        }

        return ValidationProblem(keyValuePairs);
    }
}
