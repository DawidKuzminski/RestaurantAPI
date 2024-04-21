using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Utilities;

namespace RestaurantAPI.Infrastructure.Services.Abstraction;
public interface IAccountService
{
	Task<IResult<LoginUserResponse>> LoginUserAsync(LoginUserRequest request);
	Task<IResult> RegisterUserAsync(RegisterUserRequest request);
}