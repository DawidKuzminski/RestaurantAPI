﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Core.Dto;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Services.Abstraction;

namespace RestaurantAPI.Controllers;

[Route("api/restaurants")]
[ApiController]
[Authorize]
public class RestaurantController : ControllerBase
{
	private readonly IRestaurantService _restaurantService;
	public RestaurantController(IRestaurantService restaurantService)
	{
		_restaurantService = restaurantService;
	}

	[HttpGet]
	[Authorize(Policy = "AtLeast16")]
	public async Task<ActionResult<PageResult<RestauantDto>>> GetAll([FromQuery] RestaurantGetAllQuery query)
	{
		var getAllResult = await _restaurantService.GetAllAsync(query);
		if (getAllResult.IsNotSuccess)
			return NotFound();

		return Ok(getAllResult.Data);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<IEnumerable<RestauantDto>>> GetById([FromRoute] int id)
	{
		var getResult = await _restaurantService.GetByIdAsync(id);
		if (getResult.IsNotSuccess)
			return NotFound();

		return Ok(getResult.Data);
	}

	[HttpPost]
	[Authorize(Roles = "Admin,Manager")]
	public async Task<ActionResult> CreateRestaurant([FromBody] CreateRestaurantRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var createRestaurantResult = await _restaurantService.CreateRestaurantAsync(request);
		if (createRestaurantResult.IsNotSuccess)
			return BadRequest();

		return Created($"/api/restaurant/{createRestaurantResult.Data}", null);
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin,Manager")]
	public async Task<ActionResult> DeleteRestaurant([FromRoute] int id)
	{
		var deleteRestaurantResult = await _restaurantService.DeleteRestaurantAsync(id);
		return deleteRestaurantResult.IsSuccess ? NoContent() : BadRequest();
	}

	[HttpPut("{id}")]
	[Authorize(Roles = "Admin,Manager")]
	public async Task<ActionResult> UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var updatedRestaurantResult = await _restaurantService.UpdateRestaurantAsync(id, request);
		if (updatedRestaurantResult.IsNotSuccess)
			return NotFound();

		return Ok(updatedRestaurantResult.Data);
	}
}
