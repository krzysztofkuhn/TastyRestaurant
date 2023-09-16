namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public record CreateOrderRequest(List<OrderItemRequest> OrderItems);