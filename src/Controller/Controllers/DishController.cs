using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Services.Abstraction;

namespace RestaurantAPI.Controller.Controllers;

[Route("api/restaurant/{restaurantId}/dishes")]
[ApiController]
[Authorize]
public class DishController : ControllerBase
{
	private readonly IDishService _dishService;

	public DishController(IDishService dishService)
    {
		_dishService = dishService;
	}

    [HttpPost]
	[Authorize(Roles = "Admin,Manager")]
	public async Task<ActionResult> AddDish([FromRoute] int restaurantId, [FromBody] CreateDishRequest request)
    {
		var createDishResult = await _dishService.CreateDishAsync(restaurantId, request);
		if (createDishResult.IsNotSuccess)
			return NotFound();

		return Created($"api/restaurant/{restaurantId}/dishes/{createDishResult.Data}", null);
	}

	[HttpGet("{dishId}")]
	public async Task<ActionResult<DishDto>> GetDishById([FromRoute] int restaurantId, [FromRoute] int dishId)
	{
		var getDishByIdResult = await _dishService.GetDishByIdAsync(restaurantId, dishId);
		if (getDishByIdResult.IsNotSuccess)
			return NotFound();

		return Ok(getDishByIdResult.Data);
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<DishDto>>> GetAllDishes([FromRoute] int restaurantId)
	{
		var getAllDishesResult = await _dishService.GetAllDishesAsync(restaurantId);
		if(getAllDishesResult.IsNotSuccess)
			return NotFound();

		return Ok(getAllDishesResult.Data);
	}

	[HttpDelete("{dishId}")]
	[Authorize(Roles = "Admin,Manager")]
	public async Task<ActionResult> DeleteDishById([FromRoute] int restaurantId, [FromRoute] int dishId)
	{
		var deleteDishResult = await _dishService.DeleteDishByIdAsync(restaurantId, dishId);
		if(deleteDishResult.IsNotSuccess)
			return NotFound();

		return NoContent();
	}
}
