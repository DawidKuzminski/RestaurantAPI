using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Core.DTO;

public class CreateRestaurantRequest
{
	[Required]
	[MaxLength(256)]
	public string Name { get; init; }
	public string Description { get; init; }
	public string Category { get; init; }
	public bool HasDelivery { get; init; }
	public string ContactEmail { get; init; }
	public string ContactPhone { get; init; }

	[Required]
	[MaxLength(256)]
	public string City { get; init; }

	[Required]
	[MaxLength(256)]
	public string Street { get; init; }
	public string PostalCode { get; init; }
}
