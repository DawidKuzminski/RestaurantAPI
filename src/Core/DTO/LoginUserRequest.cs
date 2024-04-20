namespace RestaurantAPI.Core.DTO;
public class LoginUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserResponse
{
    public string Token { get; init; }
}
