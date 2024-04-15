using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Core.Entity;
using RestaurantAPI.Core.Model;
using RestaurantAPI.Infrastructure.Database;
using RestaurantAPI.Infrastructure.Services.Abstraction;

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

	public bool RegisterUser(RegisterUserRequest request)
	{
		var userEntity = _mapper.Map<UserEntity>(request);
		userEntity.RoleId = (int)Role.User;
		userEntity.Password = _passwordHasher.HashPassword(userEntity, request.Password);

		_dbContext.Users.Add(userEntity);
		_dbContext.SaveChanges();

		return true;
	}

	public string LoginUser(LoginUserRequest request)
	{
		var userEntity = _dbContext.Users.FirstOrDefault(x => x.Email == request.Email);
		if (userEntity is null)
			return null;


	}

}
