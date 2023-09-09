using TastyRestaurant.WebApi.Contracts.V1.Responses;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Mappers;

public static class OrderItemMapper
{
    public static List<OrderItemResponse> MapToOrderItemResponseList(this IEnumerable<OrderItem> orderItems)
    {
        var result = orderItems
            .Select(x => x.MapToOrderItemResponse())
            .ToList();

        return result;
    }

    public static OrderItemResponse MapToOrderItemResponse(this OrderItem orderItem)
    {
        var result = new OrderItemResponse(orderItem.MenuItem.MapToMenuItemResponse(), orderItem.Quantity, orderItem.TotalPrice);

        return result;
    }
}