using TastyRestaurant.WebApi.Contracts.V1.Consts;

namespace TastyRestaurant.WebApi.Contracts.V1.Responses;

public record OrderResponse(
    Guid Id,
    OrderStatusEnumContract Status, 
    Guid UserId, 
    List<OrderItemResponse> OrderItems, 
    decimal Price, 
    DateTime CreationDate, 
    DateTime UpdateDate
    );