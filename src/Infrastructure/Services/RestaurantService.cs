using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services.Abstraction;
using RestaurantAPI.Infrastructure.Utilities;
using RestaurantAPI.Infrastructure.Validation;
using System.Linq.Expressions;
using RestaurantAPI.Core.Model;

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

	public async Task<IResult<PageResult<RestauantDto>>> GetAllAsync(RestaurantGetAllQuery query)
	{
		var queryValidator = new RestaurantGetAllQueryValidator();
		if(!queryValidator.Validate(query).IsValid)
		{
			return Result<PageResult<RestauantDto>>.Success(ResultStatusCode.BadRequest);
		}

		var container = _dbContext
			.Restaurants
			.AsNoTracking()
			.Include(r => r.Address)
			.Include(r => r.Dishes);

		int elementsToSkip = query.PageSize * (query.PageNumber - 1);

		IQueryable<RestaurantEntity> restaurantsAll = null;
		if (!string.IsNullOrEmpty(query.SearchPhrase))
			restaurantsAll = container
			.Where(x => x.Name.ToLower().Contains(query.SearchPhrase) ||
				(x.Description != null && x.Description.Contains(query.SearchPhrase)));
		else
			restaurantsAll = container;

		var sortSelectors = new Dictionary<SortBy, Expression<Func<IBaseSortItem, object>>>
		{
			{ SortBy.Default, x => x.Id },
			{ SortBy.Name, x => x.Name }
		};

		var sortSelectorExpression = sortSelectors.FirstOrDefault(x => x.Key == query.SortBy).Value;

		if (query.SortBy is not null || query.SortBy != SortBy.Default)
			restaurantsAll = query.SortDirection == SortDirection.Asc ?
				(IQueryable<RestaurantEntity>)restaurantsAll.OrderBy(sortSelectorExpression) :
				(IQueryable<RestaurantEntity>)restaurantsAll.OrderByDescending(sortSelectorExpression);

		var restaurants = await restaurantsAll
			.Skip(elementsToSkip)
			.Take(query.PageSize)
			.ToListAsync();

		if (restaurants is null)
			return Result<PageResult<RestauantDto>>.Success(ResultStatusCode.NoDataFound);

		PageResult<RestauantDto> pageResult = new(_mapper.Map<List<RestauantDto>>(restaurants), restaurantsAll.Count(), query.PageSize, query.PageNumber);

		return Result<PageResult<RestauantDto>>.Success(pageResult);
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

