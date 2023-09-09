namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public record CreateOrderRequest(Guid UserId, List<OrderItemRequest> OrderItems);