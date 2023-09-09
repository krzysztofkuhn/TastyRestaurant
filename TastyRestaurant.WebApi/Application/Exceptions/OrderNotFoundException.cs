namespace TastyRestaurant.WebApi.Application.Exceptions;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid orderId) : base($"Order with given id doesn't exist. Id: {orderId}") { }
}