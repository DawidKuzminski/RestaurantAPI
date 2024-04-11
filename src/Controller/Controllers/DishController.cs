using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Services.Abstraction;

namespace RestaurantAPI.Controller.Controllers;

[Route("api/restaurant/{restaurantId}/dish")]
[ApiController]
public class DishController : ControllerBase
{
	private readonly IDishService _dishService;

	public DishController(IDishService dishService)
    {
		_dishService = dishService;
	}

    [HttpPost]
    public ActionResult AddDish([FromRoute] int restaurantId, [FromBody] CreateDishRequest request)
    {
		var dishId = _dishService.CreateDish(restaurantId, request);
		return Created($"api/restaurant/{restaurantId}/dish/{dishId}", null);
	}

	[HttpGet("{dishId}")]
	public ActionResult<DishDto> GetDishById([FromRoute] int restaurantId, [FromRoute] int dishId)
	{
		var dishDto = _dishService.GetDishById(restaurantId, dishId);
		if (dishDto is null)
			return NotFound();

		return Ok(dishDto);
	}

	[HttpGet]
	public ActionResult<IEnumerable<DishDto>> GetAllDishes([FromRoute] int restaurantId)
	{
		var dishDtos = _dishService.GetAllDishes(restaurantId);
		if(dishDtos is null)
			return NotFound();

		return Ok(dishDtos);
	}

	[HttpDelete("{dishId}")]
	public ActionResult DeleteDishById([FromRoute] int restaurantId, [FromRoute] int dishId)
	{
		var isDeleted = _dishService.DeleteDishById(restaurantId, dishId);
		if(!isDeleted)
			return NotFound();

		return NoContent();
	}
}
