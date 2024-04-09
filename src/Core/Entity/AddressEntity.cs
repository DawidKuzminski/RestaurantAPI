namespace RestaurantAPI.Core.Entity;

public class AddressEntity
{
	public int Id { get; set; }
	public string City { get; set; }
	public string Street { get; set; }
	public string PostalCode { get; set; }
	public virtual RestaurantEntity Restaurant { get; set; }
}
