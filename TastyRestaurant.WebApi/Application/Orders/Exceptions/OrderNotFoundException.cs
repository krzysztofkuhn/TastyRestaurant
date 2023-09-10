namespace TastyRestaurant.WebApi.Application.Orders.Exceptions;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid orderId) : base($"Order with given id doesn't exist. Id: {orderId}") { }
}