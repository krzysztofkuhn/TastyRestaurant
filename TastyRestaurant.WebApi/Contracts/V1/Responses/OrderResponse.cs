using TastyRestaurant.WebApi.Contracts.V1.Consts;

namespace TastyRestaurant.WebApi.Contracts.V1.Responses;

public record OrderResponse(
    int Id,
    OrderStatusEnumContract Status, 
    int UserId, 
    List<OrderItemResponse> OrderItems, 
    decimal Price, 
    DateTime CreationDate, 
    DateTime UpdateDate
    );