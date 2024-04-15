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
        if (registerUserResult.IsNotSuccess)
            return BadRequest();

        return Ok();
	}

    [HttpPost("login")]
    public ActionResult<string> Login([FromBody] LoginUserRequest request)
    {
        var loginUserResult = _accountService.LoginUser(request);
        if(loginUserResult.IsNotSuccess)
            return BadRequest();

        return Ok(loginUserResult.Data);
    }
}
