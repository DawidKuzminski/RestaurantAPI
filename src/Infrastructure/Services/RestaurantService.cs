using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services.Abstraction;

namespace RestaurantAPI.Infrastructure.Services;

public class RestaurantService : IRestaurantService
{
	private readonly RestaurantDbContext _dbContext;
	private readonly IMapper _mapper;

	public RestaurantService(RestaurantDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public RestauantDto GetById(int id)
	{
		var restaurant = _dbContext
			.Restaurants
			.Include(r => r.Address)
			.Include(r => r.Dishes)
			.FirstOrDefault(x => x.Id == id);

		return restaurant is not null ?
			_mapper.Map<RestauantDto>(restaurant) :
			null;
	}

	public IEnumerable<RestauantDto> GetAll()
	{
		var restaurants = _dbContext
			.Restaurants
			.Include(r => r.Address)
			.Include(r => r.Dishes)
			.ToList();

		return restaurants is not null ?
			_mapper.Map<List<RestauantDto>>(restaurants) :
		null;
	}

	public int CreateRestaurant(CreateRestaurantRequest request)
	{
		var restaurant = _mapper.Map<RestaurantEntity>(request);
		_dbContext.Add(restaurant);
		_dbContext.SaveChanges();

		return restaurant.Id;
	}

	public bool DeleteRestaurant(int id)
	{
		var restaurant = _dbContext
			.Restaurants
			.FirstOrDefault(x => x.Id == id);

		if (restaurant is null)
			return false;

		_dbContext.Restaurants.Remove(restaurant);
		_dbContext.SaveChanges(); 
		
		return true;
	}

	public RestauantDto UpdateRestaurant(int id, UpdateRestaurantRequest request)
	{
		var restaurant = _dbContext
			.Restaurants
			.FirstOrDefault(x => x.Id == id);

		if (restaurant is null)
			return null;

		var updatedEntity = _mapper.Map(request, restaurant);
		_dbContext.Restaurants.Update(updatedEntity);
		_dbContext.SaveChanges();

		return _mapper.Map<RestauantDto>(updatedEntity);
	}
}

