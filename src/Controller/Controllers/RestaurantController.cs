using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services.Abstraction;

namespace RestaurantAPI.Controllers;

[Route("api/restaurants")]
public class RestaurantController : ControllerBase
{
	private readonly IRestaurantService _restaurantService;
	public RestaurantController(IRestaurantService restaurantService)
    {
		_restaurantService = restaurantService;
	}

	[HttpGet]
    public ActionResult<IEnumerable<RestauantDto>> GetAll()
	{
		var getAllRestaurants = _restaurantService.GetAll();
		if (getAllRestaurants is null)
			return NotFound();

		return Ok(getAllRestaurants);
	}

	[HttpGet("{id}")]
	public ActionResult<IEnumerable<RestauantDto>> GetById([FromRoute] int id)
	{
		var restaurant = _restaurantService.GetById(id);
		if (restaurant is null)
			return NotFound();		

		return Ok(restaurant);
	}

	[HttpPost]
	public ActionResult CreateRestaurant([FromBody] CreateRestaurantRequest request)
	{
		if(!ModelState.IsValid)
			return BadRequest(ModelState);

		var restaurantId = _restaurantService.CreateRestaurant(request);

		return Created($"/api/restaurant/{restaurantId}", null);
	}

	[HttpDelete("{id}")]
	public ActionResult DeleteRestaurant([FromRoute] int id)
	{
		var isDeleted = _restaurantService.DeleteRestaurant(id);
		return isDeleted ? NoContent() : BadRequest();
	}

	[HttpPut("{id}")]
	public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantRequest request)
	{
		if(!ModelState.IsValid)
			return BadRequest(ModelState);

		var updatedRestaurant = _restaurantService.UpdateRestaurant(id, request);
		return Ok(updatedRestaurant);
	}
}
