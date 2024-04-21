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
	public async Task<ActionResult> RegisterUser([FromBody] RegisterUserRequest request)
	{
        var registerUserResult = await _accountService.RegisterUserAsync(request);
        if (registerUserResult.IsNotSuccess)
            return BadRequest();

        return Ok();
	}

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserRequest request)
    {
        var loginUserResult = await _accountService.LoginUserAsync(request);
        if(loginUserResult.IsNotSuccess)
            return BadRequest();

        return Ok(loginUserResult.Data);
    }
}
