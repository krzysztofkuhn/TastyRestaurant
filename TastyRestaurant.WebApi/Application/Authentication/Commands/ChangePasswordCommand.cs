using MediatR;
using Microsoft.AspNetCore.Identity;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Application.Authentication.Commands;

public record ChangePasswordCommand(Guid UserId, string OldPassword, string NewPassword) : IRequest<ChangePasswordCommand>, IRequest;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.OldPassword))
            throw new OldPasswordIncorrectException();

        await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
    }
}