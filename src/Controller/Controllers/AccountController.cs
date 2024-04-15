using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Core.DTO;
using RestaurantAPI.Infrastructure.Services.Abstraction;

namespace RestaurantAPI.Controller.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
	public ActionResult RegisterUser([FromBody] RegisterUserRequest request)
	{
        var registerUserResult = _accountService.RegisterUser(request);
        return Ok();
	}

    [HttpPost("login")]
    public ActionResult Login([FromBody] LoginUserRequest request)
    {

    }
}
