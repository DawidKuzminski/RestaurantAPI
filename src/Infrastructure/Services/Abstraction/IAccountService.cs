using RestaurantAPI.Core.DTO;

namespace RestaurantAPI.Infrastructure.Services.Abstraction;
public interface IAccountService
{
    bool RegisterUser(RegisterUserRequest request);
}