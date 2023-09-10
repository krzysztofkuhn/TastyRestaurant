using MediatR;
using Microsoft.AspNetCore.Identity;
using TastyRestaurant.WebApi.Application.Authentication.Models;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Application.Authentication.Commands;

public record LoginCommand(string Email, string Password) : IRequest<LoginCommandResponse>;

public record LoginCommandResponse(bool Success, GenerateJwtTokenResponse? TokenResponse);

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public LoginCommandHandler(UserManager<ApplicationUser> userManager, IJwtTokenProvider jwtTokenProvider)
    {
        _userManager = userManager;
        _jwtTokenProvider = jwtTokenProvider;
    }

    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return new LoginCommandResponse(false, null);

        var userRoles = await _userManager.GetRolesAsync(user);

        var response = _jwtTokenProvider.Generate(user, userRoles);

        return new LoginCommandResponse(true, response);
    }
}