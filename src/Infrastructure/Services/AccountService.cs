using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Core.Model;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services.Abstraction;
using RestaurantAPI.Infrastructure.Utilities;

namespace RestaurantAPI.Infrastructure.Services;
public class AccountService : IAccountService
{
	private readonly RestaurantDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly IPasswordHasher<UserEntity> _passwordHasher;

	public AccountService(RestaurantDbContext dbContext, IMapper mapper, IPasswordHasher<UserEntity> passwordHasher)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_passwordHasher = passwordHasher;
	}

	public IResult RegisterUser(RegisterUserRequest request)
	{
		bool isUserAlreadyExist = _dbContext.Users.Any(x => x.Email == request.Email);
		if (isUserAlreadyExist)
			return Result.Success(ResultStatusCode.DataAlreadyExist);

		var userEntity = _mapper.Map<UserEntity>(request);
		userEntity.RoleId = (int)Role.User;
		userEntity.Password = _passwordHasher.HashPassword(userEntity, request.Password);

		_dbContext.Users.Add(userEntity);
		_dbContext.SaveChanges();

		return Result.Success();
	}

	public IResult<string> LoginUser(LoginUserRequest request)
	{
		var userEntity = _dbContext.Users.FirstOrDefault(x => x.Email == request.Email);
		if (userEntity is null)
			return Result<string>.Success(null, ResultStatusCode.NoDataFound);


	}

}
