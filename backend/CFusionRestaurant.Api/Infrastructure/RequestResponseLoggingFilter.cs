using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CFusionRestaurant.Api.Infrastructure;

/// <summary>
/// Action filter for logging incoming requests and outgoing responses.
/// </summary>
public class RequestResponseLoggingFilter : IAsyncActionFilter
{
    private readonly ILogger<RequestResponseLoggingFilter> _logger;

    public RequestResponseLoggingFilter(ILogger<RequestResponseLoggingFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Log request details
        _logger.LogInformation($"Incoming request: {context.HttpContext.Request.Method} {context.HttpContext.Request.Path}");

        if (context.HttpContext.Request.Body.CanRead)
        {
            context.HttpContext.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
            _logger.LogInformation($"Request body: {requestBody}");
            context.HttpContext.Request.Body.Position = 0;
        }

        var resultContext = await next();

        // Log response details
        var result = resultContext.Result as ObjectResult;
        if (result != null)
        {
            _logger.LogInformation($"Outgoing response: {result.StatusCode}");
            _logger.LogInformation("Response body: {@responseBody}", result.Value);
        }
    }
}
