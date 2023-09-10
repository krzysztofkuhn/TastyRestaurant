using MediatR;
using Microsoft.AspNetCore.Identity;
using TastyRestaurant.WebApi.Application.Authentication.Exceptions;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Application.Authentication.Commands;

public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName, string PhoneNumber, DateTime? DateOfBirth) : IRequest<string>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(RegisterUserCommand model, CancellationToken cancellationToken)
    {
        var userExists = await _userManager.FindByNameAsync(model.Email);
        if (userExists != null)
            throw new UserAlreadyExistsException();

        var now = DateTime.Now;
        ApplicationUser user = new()
        {
            UserName = model.Email,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreationDate = now,
            UpdateDate = now,
            DateOfBirth = model.DateOfBirth,
            PhoneNumber = model.PhoneNumber,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new UserCreationFailedException();

        return user.Id;
    }
}