using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.Entities;
using RestaurantAPI.Infrastructure.Database;

namespace RestaurantAPI.Controllers;

[Route("api/restaurants")]
public class RestaurantController : ControllerBase
{
    private readonly RestaurantDbContext _dbContext;
	private readonly IMapper _mapper;

	public RestaurantController(RestaurantDbContext dbContext, IMapper mapper)
    {
		_dbContext = dbContext;
		_mapper = mapper;
	}

	[HttpGet]
    public ActionResult<IEnumerable<RestauantDto>> GetAll()
	{
		var restaurants = _dbContext
			.Restaurants
			.Include(r => r.Address)
			.Include(r => r.Dishes)
			.ToList();

		var restaurantDtos = _mapper.Map<List<RestauantDto>>(restaurants);

		return Ok(restaurantDtos);
	}

	[HttpGet("{id}")]
	public ActionResult<IEnumerable<RestauantDto>> GetById([FromRoute] int id)
	{
		var restaurant = _dbContext
			.Restaurants
			.Include(r => r.Address)
			.Include(r => r.Dishes)
			.FirstOrDefault(x => x.Id == id);

		if(restaurant is null)
			return NotFound();

		var restaurantDto = _mapper.Map<RestauantDto>(restaurant);

		return Ok(restaurantDto);
	}
}
