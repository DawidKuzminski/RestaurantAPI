using RestaurantAPI.Core.Entities;

namespace RestaurantAPI.Core.Dto;

public class DishDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public decimal Price { get; set; }
}
