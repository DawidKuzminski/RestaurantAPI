using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;

namespace RestaurantAPI.Infrastructure.Services.Abstraction;

public interface IRestaurantService
{
	int CreateRestaurant(CreateRestaurantRequest request);
	bool DeleteRestaurant(int id);
	IEnumerable<RestauantDto> GetAll();
	RestauantDto GetById(int id);
	RestauantDto UpdateRestaurant(int id, UpdateRestaurantRequest request);
}