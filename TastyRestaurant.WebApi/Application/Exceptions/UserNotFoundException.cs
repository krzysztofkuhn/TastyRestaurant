namespace TastyRestaurant.WebApi.Application.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid id) : base($"User with given id doesn't exist. Id: {id}") { }
}