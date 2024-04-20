using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Core.Model;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services.Abstraction;
using RestaurantAPI.Infrastructure.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Infrastructure.Services;
public class AccountService : IAccountService
{
	private readonly RestaurantDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly IPasswordHasher<UserEntity> _passwordHasher;
	private readonly AuthenticationSettings _authenticationSettings;

	public AccountService(RestaurantDbContext dbContext, IMapper mapper, IPasswordHasher<UserEntity> passwordHasher, AuthenticationSettings authenticationSettings)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_passwordHasher = passwordHasher;
		_authenticationSettings = authenticationSettings;
	}

	public IResult RegisterUser(RegisterUserRequest request)
	{
		bool isUserAlreadyExist = _dbContext.Users.Any(x => x.Email == request.Email);
		if (isUserAlreadyExist)
			return Result.Success(ResultStatusCode.DataAlreadyExist);

		var userEntity = _mapper.Map<UserEntity>(request);
		userEntity.Password = _passwordHasher.HashPassword(userEntity, request.Password);

		var userRole = _dbContext.Roles.FirstOrDefault(x => x.Role == Role.User);
		if (userRole is null)
			throw new NullReferenceException($"Cannot found role: {Role.User} - {Role.User.ToString()}");
		userEntity.RoleId = userRole.Id;
		userEntity.Role = userRole;		

		_dbContext.Users.Add(userEntity);
		_dbContext.SaveChanges();

		return Result.Success();
	}

	public IResult<string> LoginUser(LoginUserRequest request)
	{
		var userEntity = _dbContext
			.Users
			.Include(x => x.Role)
			.FirstOrDefault(x => x.Email == request.Email);
		if (userEntity is null)
			return Result<string>.Success(ResultStatusCode.NoDataFound);

		var verifyPasswordResult = _passwordHasher.VerifyHashedPassword(userEntity, userEntity.Password, request.Password);
		if (verifyPasswordResult == PasswordVerificationResult.Failed)
			return Result<string>.Success(ResultStatusCode.NoDataFound);

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, userEntity.Id.ToString()),
			new Claim(ClaimTypes.Name, $"{userEntity.FirstName} {userEntity.LastName}"),
			new Claim(ClaimTypes.Email, userEntity.Email),
			new Claim(ClaimTypes.Role, userEntity.Role.Role.ToString()),
			new Claim("nationality", userEntity.Nationality)
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.Jwk));
		var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
		var expires = DateTime.UtcNow.AddDays(_authenticationSettings.JwtExpireDays);

		var securityToken = new JwtSecurityToken(_authenticationSettings.Issuer,
			_authenticationSettings.Issuer,
			claims,
			expires: expires,
			signingCredentials: cred);

		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.WriteToken(securityToken);

		return Result<string>.Success(token);
	}

}
