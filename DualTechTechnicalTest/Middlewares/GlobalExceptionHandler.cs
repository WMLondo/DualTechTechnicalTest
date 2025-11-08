using DualTechTechnicalTest.Domain.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DualTechTechnicalTest.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler > logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        var response = Result<GlobalExceptionHandler >.FailureResponse("Something went wrong.");

        await httpContext.Response
            .WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}