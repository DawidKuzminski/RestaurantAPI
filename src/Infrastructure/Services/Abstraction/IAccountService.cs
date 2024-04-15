using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Utilities;

namespace RestaurantAPI.Infrastructure.Services.Abstraction;
public interface IAccountService
{
	public IResult RegisterUser(RegisterUserRequest request);
}