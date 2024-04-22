using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services.Abstraction;
using RestaurantAPI.Infrastructure.Utilities;
using RestaurantAPI.Infrastructure.Validation;

namespace RestaurantAPI.Infrastructure.Services;
public class RestaurantService : IRestaurantService
{
	private readonly RestaurantDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly ILogger<RestaurantService> _logger;
	private readonly IAuthorizationService _authorizationService;
	private readonly IUserContextService _userContextService;

	public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService, IUserContextService userContextService)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_logger = logger;
		_authorizationService = authorizationService;
		_userContextService = userContextService;
	}

	public async Task<IResult<RestauantDto>> GetByIdAsync(int id)
	{
		var restaurant = await _dbContext
			.Restaurants
			.Include(r => r.Address)
			.Include(r => r.Dishes)
			.FirstOrDefaultAsync(x => x.Id == id);

		if (restaurant is null)
			return Result<RestauantDto>.Success(ResultStatusCode.NoDataFound);

		return Result<RestauantDto>.Success(_mapper.Map<RestauantDto>(restaurant));
	}

	public async Task<IResult<IEnumerable<RestauantDto>>> GetAllAsync()
	{
		var restaurants = await _dbContext
			.Restaurants
			.Include(r => r.Address)
			.Include(r => r.Dishes)
			.ToListAsync();

		if (restaurants is null)
			return Result<IEnumerable<RestauantDto>>.Success(ResultStatusCode.NoDataFound);

		return Result<IEnumerable<RestauantDto>>.Success(_mapper.Map<List<RestauantDto>>(restaurants));
	}

	public async Task<Utilities.IResult<CreateResourceResponse>> CreateRestaurantAsync(CreateRestaurantRequest request)
	{
		var requestUserId = _userContextService.GetUserId;
		if (requestUserId is null)
			return Result<CreateResourceResponse>.Success(ResultStatusCode.AccessForbidden);

		var restaurant = _mapper.Map<RestaurantEntity>(request);
		restaurant.OwnerId = requestUserId.Value;

		var entityEntry = await _dbContext.AddAsync(restaurant);
		await _dbContext.SaveChangesAsync();

		return Result<CreateResourceResponse>.Success(new CreateResourceResponse { Id = entityEntry.Entity.Id });
	}

	public async Task<Utilities.IResult> DeleteRestaurantAsync(int id)
	{
		var restaurant = await _dbContext
			.Restaurants
			.FirstOrDefaultAsync(x => x.Id == id);

		if (restaurant is null)
			return Result.Success(ResultStatusCode.NoDataFound);

		var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ValidateResourceOperationRequirement(ResourceOperation.Delete));
		if (!authorizationResult.Succeeded)
			return Result<RestauantDto>.Success(ResultStatusCode.AccessForbidden);

		_dbContext.Restaurants.Remove(restaurant);
		await _dbContext.SaveChangesAsync();

		return Result.Success();
	}

	public async Task<IResult<RestauantDto>> UpdateRestaurantAsync(int id, UpdateRestaurantRequest request)
	{
		var restaurant = await _dbContext
			.Restaurants
			.FirstOrDefaultAsync(x => x.Id == id);

		if (restaurant is null)
			return Result<RestauantDto>.Success(ResultStatusCode.NoDataFound);

		var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ValidateResourceOperationRequirement(ResourceOperation.Update));
		if (!authorizationResult.Succeeded)
			return Result<RestauantDto>.Success(ResultStatusCode.AccessForbidden);

		var updatedEntity = _mapper.Map(request, restaurant);
		_dbContext.Restaurants.Update(updatedEntity);
		await _dbContext.SaveChangesAsync();

		return Result<RestauantDto>.Success(_mapper.Map<RestauantDto>(updatedEntity));
	}
}

