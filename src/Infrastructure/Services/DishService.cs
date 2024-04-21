using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services.Abstraction;
using RestaurantAPI.Infrastructure.Utilities;
using IResult = RestaurantAPI.Infrastructure.Utilities.IResult;

namespace RestaurantAPI.Infrastructure.Services;
public class DishService : IDishService
{
	private readonly RestaurantDbContext _dbContext;
	private readonly IMapper _mapper;

	public DishService(RestaurantDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<IResult<CreateResourceResponse>> CreateDishAsync(int restaurantId, CreateDishRequest request)
	{
		var restaurantEntity = await _dbContext
			.Restaurants
			.FirstOrDefaultAsync(x => x.Id == restaurantId);

		if (restaurantEntity is null)
			return Result<CreateResourceResponse>.Success(ResultStatusCode.NoDataFound);

		var dishEntity = _mapper.Map<DishEntity>(request);
		dishEntity.RestaurantId = restaurantId;

		await _dbContext.Dishes.AddAsync(dishEntity);
		await _dbContext.SaveChangesAsync();

		return Result<CreateResourceResponse>.Success(new CreateResourceResponse { Id = dishEntity.Id });
	}

	public async Task<IResult<DishDto>> GetDishByIdAsync(int restaurantId, int dishId)
	{
		var restaurantEntity = await _dbContext
			.Restaurants
			.Include(x => x.Dishes)
			.FirstOrDefaultAsync(x => x.Id == restaurantId);

		if (restaurantEntity is null)
			return Result<DishDto>.Success(ResultStatusCode.NoDataFound);

		var dishEntity = restaurantEntity
			.Dishes
			.FirstOrDefault(x => x.Id == dishId);

		if (dishEntity is null)
			return Result<DishDto>.Success(ResultStatusCode.NoDataFound);

		var dishDto = _mapper.Map<DishDto>(dishEntity);
		return Result<DishDto>.Success(dishDto);
	}

	public async Task<IResult<IEnumerable<DishDto>>> GetAllDishesAsync(int restaurantId)
	{
		var restaurantEntity = await _dbContext
			.Restaurants
			.Include(x => x.Dishes)
			.FirstOrDefaultAsync(x => x.Id == restaurantId);

		if (restaurantEntity is null)
			return Result<IEnumerable<DishDto>>.Success(ResultStatusCode.NoDataFound);

		var dishDtos = _mapper.Map<IEnumerable<DishDto>>(restaurantEntity.Dishes);
		return Result<IEnumerable<DishDto>>.Success(dishDtos);
	}

	public async Task<IResult> DeleteDishByIdAsync(int restaurantId, int dishId)
	{
		var restaurantEntity = await _dbContext
			.Restaurants
			.Include(x => x.Dishes)
			.FirstOrDefaultAsync(x => x.Id == restaurantId);

		if (restaurantEntity is null)
			return Result.Success(ResultStatusCode.NoDataFound);

		var dishEntity = restaurantEntity
			.Dishes
			.FirstOrDefault(x => x.Id == dishId);

		if (dishEntity is null)
			return Result.Success(ResultStatusCode.NoDataFound);

		_dbContext.Dishes.Remove(dishEntity);
		await _dbContext.SaveChangesAsync();

		return Result.Success();
	}
}
