using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Utilities;

namespace RestaurantAPI.Infrastructure.Services.Abstraction;
public interface IAccountService
{
	IResult<string> LoginUser(LoginUserRequest request);
	public IResult RegisterUser(RegisterUserRequest request);
}