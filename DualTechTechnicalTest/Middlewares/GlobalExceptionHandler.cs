using DualTechTechnicalTest.Domain.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DualTechTechnicalTest.Middlewares;

public class GlobalExceptionHandler(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger) : IExceptionHandler
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, "Exception occurred: {Message}", ex.Message);

            var response = Result<ExceptionHandlingMiddleware>.FailureResponse("Server Error");

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
    {
        logger.LogError(
            ex, "Exception occurred: {Message}", ex.Message);

        var response = Result<ExceptionHandlingMiddleware>.FailureResponse("Server Error");

        await httpContext.Response
            .WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}