using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Core.DTO;
public class RegisterUserRequest
{
	[Required]
	public string Email { get; init; }

	[Required]
	[MinLength(8)]
	public string Password { get; init; }

	[MaxLength(50)]
	public string FirstName { get; init; }

	[MaxLength(50)]
	public string LastName { get; init; }

	[MaxLength(50)]
	public string Nationality { get; init; }

	public DateTime? DateOfBirth { get; init; }
}
