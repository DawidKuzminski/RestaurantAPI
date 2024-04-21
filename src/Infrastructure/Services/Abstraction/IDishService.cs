using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Utilities;

namespace RestaurantAPI.Infrastructure.Services.Abstraction;

public interface IDishService
{
	Task<IResult<CreateResourceResponse>> CreateDishAsync(int restaurantId, CreateDishRequest request);
	Task<IResult> DeleteDishByIdAsync(int restaurantId, int dishId);
	Task<IResult<IEnumerable<DishDto>>> GetAllDishesAsync(int restaurantId);
	Task<IResult<DishDto>> GetDishByIdAsync(int restaurantId, int dishId);
}