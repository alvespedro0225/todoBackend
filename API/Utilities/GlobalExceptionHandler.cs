using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Utilities;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
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
            case HttpException apiException:
                response.StatusCode = apiException.StatusCode;
                problemDetails = CreateProblemDetails(apiException);
                break;
        }

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });
    }

    private static ProblemDetails CreateProblemDetails(HttpException exception)
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
}