using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;

namespace RestaurantAPI.Infrastructure.Services.Abstraction;

public interface IDishService
{
	int CreateDish(int restaurantId, CreateDishRequest request);
	bool DeleteDishById(int restaurantId, int dishId);
	IEnumerable<DishDto> GetAllDishes(int restaurantId);
	DishDto GetDishById(int restaurantId, int dishId);
}