namespace TastyRestaurant.WebApi.Application.Users.Commands;

public record UpdateUserCommand(string Email, string FirstName, string LastName, string PhoneNumber, DateTime? DateOfBirth);