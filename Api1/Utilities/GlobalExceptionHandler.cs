using System.Text;
using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Utilities;

public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = exception.GetType().ToString(),
            Title = "There was an issue with the server"
        };
        
        var response = httpContext.Response;
        
        switch (exception)
        {
            case IHttpException httpException:
                response.StatusCode = httpException.StatusCode;
                problemDetails = CreateProblemDetails(httpException);
                break;
            
            case BadHttpRequestException badRequestException:
                response.StatusCode = badRequestException.StatusCode;
                problemDetails.Status = badRequestException.StatusCode;
                problemDetails.Type = "Bad Request";
                problemDetails.Detail = badRequestException.Message;
                problemDetails.Title = "There was an issue with the requests";
                break;
            
            case ValidationException validationException:
                response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Type = "Bad Request";
                problemDetails.Detail = GetValidationMessages(validationException.Errors);
                problemDetails.Title = "There was an issue validating the item";
                break;
        }

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });
    }

    private static ProblemDetails CreateProblemDetails(IHttpException exception)
    {
        var problemDetails = new ProblemDetails
        {
            Status = exception.StatusCode,
            Title = exception.Error,
            Detail = exception.Message,
            Type = exception.Type
        };
        return problemDetails;
    }

    private static string GetValidationMessages(IEnumerable<ValidationFailure> validationExceptionErrors)
    {
        var message = new StringBuilder();
        foreach (var validationFailure in validationExceptionErrors)
        {
            message.AppendLine(validationFailure.ErrorMessage);
        }

        return message.ToString();
    }
}