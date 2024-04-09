namespace RestaurantAPI.Core.Entity;

public class RestaurantEntity
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Category { get; set; }
	public bool HasDelivery { get; set; }
	public string ContactEmail { get; set; }
	public string ContactPhone { get; set; }
	public int AddressId { get; set; }
	public virtual AddressEntity Address { get; set; }
	public virtual List<DishEntity> Dishes { get; set; }
}
