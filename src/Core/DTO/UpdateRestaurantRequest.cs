using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Core.DTO;
public class UpdateRestaurantRequest
{
    [MaxLength(256)]
    public string Name { get; init; }
    public string Description { get; init; }
    public bool? HasDelivery { get; init; }
}
