namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public record CreateOrderRequest(int UserId, List<OrderItemRequest> OrderItems);