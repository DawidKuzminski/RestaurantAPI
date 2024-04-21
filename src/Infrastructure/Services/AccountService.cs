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

	public async Task<IResult> RegisterUserAsync(RegisterUserRequest request)
	{
		bool isUserAlreadyExist = await _dbContext.Users.AnyAsync(x => x.Email == request.Email);
		if (isUserAlreadyExist)
			return Result.Success(ResultStatusCode.DataAlreadyExist);

		var userEntity = _mapper.Map<UserEntity>(request);
		userEntity.Password = _passwordHasher.HashPassword(userEntity, request.Password);

		var userRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Role == Role.User);
		if (userRole is null)
			return Result.Success(ResultStatusCode.DataAlreadyExist, $"Cannot found role: {Role.User} - {Role.User.ToString()}");

		userEntity.RoleId = userRole.Id;
		userEntity.Role = userRole;		

		await _dbContext.Users.AddAsync(userEntity);
		await _dbContext.SaveChangesAsync();

		return Result.Success();
	}

	public async Task<IResult<LoginUserResponse>> LoginUserAsync(LoginUserRequest request)
	{
		var userEntity = await _dbContext
			.Users
			.Include(x => x.Role)
			.FirstOrDefaultAsync(x => x.Email == request.Email);

		if (userEntity is null)
			return Result<LoginUserResponse>.Success(ResultStatusCode.NoDataFound);

		var verifyPasswordResult = _passwordHasher.VerifyHashedPassword(userEntity, userEntity.Password, request.Password);
		if (verifyPasswordResult == PasswordVerificationResult.Failed)
			return Result<LoginUserResponse>.Success(ResultStatusCode.NoDataFound);

		var claims = new List<Claim>
		{
			new (ClaimTypes.NameIdentifier, userEntity.Id.ToString()),
			new (ClaimTypes.Name, $"{userEntity.FirstName} {userEntity.LastName}"),
			new (ClaimTypes.Email, userEntity.Email),
			new (ClaimTypes.Role, userEntity.Role.Role.ToString()),
			new ("DateOfBirth", userEntity.DateOfBirth.Value.ToString()),
			new ("nationality", userEntity.Nationality)
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

		return Result<LoginUserResponse>.Success(new LoginUserResponse { Token = token });
	}

}
