namespace RestaurantAPI.Infrastructure;
public class AuthenticationSettings
{
    public string Jwk { get; init; }
    public string Issuer { get; init; }
    public int JwtExpireDays { get; set; }
}
