using CFusionRestaurant.ViewModel.ExceptionManagement;
using System.Net;
using System.Text.Json;

namespace CFusionRestaurant.Api.Infrastructure;

/// <summary>
/// Middleware catches all exceptions and classified them concerning their types. 
/// If the exception is a business exception it return 400 bad request with a json object that includes the message, 
/// if the exception is a not found exception it return 404 not found with a json object that includes the message, 
/// otherwise log the error and returns 500 internal server error with a json object that includes the message and a tracking id to match the errors and to find from the logs.
/// </summary>
public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionMiddleware> _logger;

    public CustomExceptionMiddleware(RequestDelegate next,
        ILogger<CustomExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex).ConfigureAwait(false);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.Clear();
        httpContext.Response.ContentType = "application/json";

        switch (exception)
        {
            case BusinessException businessException:
                await HandleBusinessException(businessException, httpContext);
                break;
            case NotFoundException notFoundException:
                await HandleNotFoundException(notFoundException, httpContext);
                break;
            default:
                await HandleGenericException(exception, httpContext);
                break;
        }
    }

    private async Task HandleBusinessException(BusinessException businessException, HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var result = JsonSerializer.Serialize(new
        {
            message = businessException.Message,
        });

        await httpContext.Response.WriteAsync(result).ConfigureAwait(false);
    }

    private async Task HandleNotFoundException(NotFoundException notFoundException, HttpContext httpContext)
    {

        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

        var result = JsonSerializer.Serialize(new
        {
            message = notFoundException.Message
        });
        await httpContext.Response.WriteAsync(result).ConfigureAwait(false);
    }

    private async Task HandleGenericException(Exception exception, HttpContext httpContext)
    {
        var trackingId = Guid.NewGuid().ToString();
        _logger.LogError(exception, "An exception has occurred. TrackingId: {TrackingId}", trackingId);

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var result = JsonSerializer.Serialize(new
        {
            TrackingId = trackingId,
            Message = "An exception has occurred"
        });

        await httpContext.Response.WriteAsync(result).ConfigureAwait(false);
    }

}
