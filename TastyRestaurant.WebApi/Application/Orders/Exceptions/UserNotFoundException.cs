namespace TastyRestaurant.WebApi.Application.Orders.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid id) : base($"User with given id doesn't exist. Id: {id}") { }
}