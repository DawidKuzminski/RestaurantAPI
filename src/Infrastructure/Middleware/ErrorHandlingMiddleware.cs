using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Core.Model;

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
			var xx = context.Request.Headers.Authorization;
			await next.Invoke(context);
		}
		catch (Exception e)
		{
			_logger.LogError(e, e.Message);
			context.Response.StatusCode = 500;
		}
	}

	private string GetRoles(string roles)
	{
		int[] splitedRoles = roles.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
		List<string> roleNames = new();
		foreach (var intRole in splitedRoles) 
		{
			Role role = (Role)intRole;
			roleNames.Add(role.ToString());
		}

		return string.Join(",", roleNames);
	}
}
