namespace TastyRestaurant.WebApi.Domain.Exceptions;

public class OrderAlreadyProcessedException : Exception
{
    public OrderAlreadyProcessedException(string message) : base(message)
    {
    }
}