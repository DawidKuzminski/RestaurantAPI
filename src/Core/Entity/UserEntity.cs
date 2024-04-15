namespace RestaurantAPI.Core.Entity;
public class UserEntity
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Nationality { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public virtual RoleEntity Role { get; set; }
}
