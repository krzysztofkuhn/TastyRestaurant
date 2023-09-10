using MediatR;
using Microsoft.AspNetCore.Mvc;
using TastyRestaurant.WebApi.Application.Authentication.Commands;
using TastyRestaurant.WebApi.Application.Authentication.Models;
using TastyRestaurant.WebApi.Contracts.V1;

namespace TastyRestaurant.WebApi.Controllers.V1;

[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthenticateController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route(ApiRoutes.Auth.Login)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        LoginCommand loginCommand = new LoginCommand(model.Email, model.Password);
        LoginCommandResponse response = await _mediator.Send(loginCommand);

        if (!response.Success) return Unauthorized();

        return Ok(new
        {
            token = response.TokenResponse.Token,
            expiration = response.TokenResponse.ValidTo
        });
    }

    [HttpPost]
    [Route(ApiRoutes.Auth.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        RegisterUserCommand registerUserCommand = new RegisterUserCommand(model.Email, model.Password, model.FirstName, model.LastName, model.PhoneNumber, model.DateOfBirth);
        await _mediator.Send(registerUserCommand);

        return Ok();
    }
}