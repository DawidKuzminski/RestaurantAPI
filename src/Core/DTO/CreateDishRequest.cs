using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Core.DTO;
public class CreateDishRequest
{
	[Required]
	public string Name { get; init; }
	public string Description { get; init; }
	public decimal Price { get; init; }
}
