using TastyRestaurant.WebApi.Contracts.V1.Consts;

namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public record UpdateOrderRequest(OrderStatusEnumContract Status, IEnumerable<OrderItemRequest> OrderItems);