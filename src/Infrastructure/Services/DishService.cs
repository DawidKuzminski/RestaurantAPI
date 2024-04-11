using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services.Abstraction;

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

    public int CreateDish(int restaurantId, CreateDishRequest request)
    {
        var restaurantEntity = _dbContext
            .Restaurants
            .FirstOrDefault(x => x.Id == restaurantId);

        if(restaurantEntity is null)
            return -1;

        var dishEntity = _mapper.Map<DishEntity>(request);
        dishEntity.RestaurantId = restaurantId;

        _dbContext.Dishes.Add(dishEntity);
        _dbContext.SaveChanges();

        return dishEntity.Id;
    }

    public DishDto GetDishById(int restaurantId, int dishId)
    {
        var restaurantEntity = _dbContext
            .Restaurants
            .Include(x => x.Dishes)
            .FirstOrDefault(x => x.Id == restaurantId);

        if (restaurantEntity is null)
            return null;

        var dishEntity = restaurantEntity
			.Dishes
            .FirstOrDefault(x => x.Id == dishId);

        if (dishEntity is null)
            return null;

        var dishDto = _mapper.Map<DishDto>(dishEntity);
        return dishDto;
    }

    public IEnumerable<DishDto> GetAllDishes(int restaurantId)
    {
		var restaurantEntity = _dbContext
			.Restaurants
            .Include(x => x.Dishes)
			.FirstOrDefault(x => x.Id == restaurantId);

		if (restaurantEntity is null)
			return null;

        var dishDtos = _mapper.Map<IEnumerable<DishDto>>(restaurantEntity.Dishes);
        return dishDtos;
	}

    public bool DeleteDishById(int restaurantId, int dishId)
    {
        var restaurantEntity = _dbContext
            .Restaurants
            .Include(x => x.Dishes)
            .FirstOrDefault(x => x.Id == restaurantId);

        if(restaurantEntity is null)
            return false;

        var dishEntity = restaurantEntity
            .Dishes
            .FirstOrDefault(x => x.Id == dishId);

        if(dishEntity is null)
            return false;

        _dbContext.Dishes.Remove(dishEntity);
        _dbContext.SaveChanges();

        return true;
    }
}
