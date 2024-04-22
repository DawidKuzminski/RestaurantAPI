using System.Security.Claims;

namespace RestaurantAPI.Infrastructure.Services;
public interface IUserContextService
{
	int? GetUserId { get; }
	ClaimsPrincipal User { get; }
}