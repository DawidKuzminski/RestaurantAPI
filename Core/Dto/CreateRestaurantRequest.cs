namespace RestaurantAPI.Core.Dto;

public class CreateRestaurantRequest
{
	public string Name { get; init; }
	public string Description { get; init; }
	public string Category { get; init; }
	public bool HasDelivery { get; init; }
	public string ContactEmail { get; init; }
	public string ContactPhone { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string PostalCode { get; init; }
}
