using TastyRestaurant.WebApi.Contracts.V1.Responses;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Mappers;

public static class OrdersMapper
{
    public static List<OrderResponse> MapToOrderResponseList(this IEnumerable<Order> orders)
    {
        var result = orders
            .Select(x => x.MapToOrderResponse())
            .ToList();

        return result;
    }

    public static OrderResponse MapToOrderResponse(this Order order)
    {
        var result = new OrderResponse(
            order.Id,
            order.Status.MapToOrderStatusEnum(),
            order.UserId,
            order.OrderItems.MapToOrderItemResponseList(),
            order.Price,
            order.CreationDate,
            order.UpdateDate
        );

        return result;
    }
}