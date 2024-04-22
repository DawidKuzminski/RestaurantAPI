using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Utilities;

namespace RestaurantAPI.Infrastructure.Services.Abstraction;

public interface IRestaurantService
{
	Task<IResult<CreateResourceResponse>> CreateRestaurantAsync(CreateRestaurantRequest request);
	Task<IResult> DeleteRestaurantAsync(int id);
	Task<IResult<IEnumerable<RestauantDto>>> GetAllAsync();
	Task<IResult<RestauantDto>> GetByIdAsync(int id);
	Task<IResult<RestauantDto>> UpdateRestaurantAsync(int id, UpdateRestaurantRequest request);
}