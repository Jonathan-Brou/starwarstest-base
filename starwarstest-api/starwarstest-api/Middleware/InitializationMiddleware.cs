// <summary>

using Domain.Interfaces;
/// Middleware to block incoming HTTP requests until the application has been fully initialized.
/// </summary>
namespace starwarstest_api.middleware{
public class InitializationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IInitializationService _initializationService;

    /// <summary>
    /// Constructor for InitializationMiddleware.
    /// </summary>
    /// <param name="next">Delegate to the next middleware in the pipeline.</param>
    /// <param name="initializationService">Service to check whether the application has been initialized.</param>
    public InitializationMiddleware(RequestDelegate next, IInitializationService initializationService)
    {
        _next = next;
        _initializationService = initializationService;
    }

    /// <summary>
    /// Process an individual request.
    /// </summary>
    /// <param name="context">The HttpContext for the current request.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (!_initializationService.InitializationComplete)
        {
            // If the initialization process is not complete, we return a "Service Unavailable" status.
            // This blocks incoming requests until initialization has been completed.
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Service is starting. Please try again later.");
        }
        else
        {
            // If the initialization is complete, we call the next middleware in the pipeline.
            await _next(context);
        }
    }
}
}