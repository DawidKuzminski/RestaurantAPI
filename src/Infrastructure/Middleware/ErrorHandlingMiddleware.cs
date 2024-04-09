using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RestaurantAPI.Infrastructure.Middleware;
public class ErrorHandlingMiddleware : IMiddleware
{
	private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next.Invoke(context);
		}
		catch (Exception e)
		{
			_logger.LogError(e, e.Message);
			context.Response.StatusCode = 500;
		}
	}
}
