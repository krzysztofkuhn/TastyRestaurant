using TastyRestaurant.WebApi.Contracts.V1.Consts;
using TastyRestaurant.WebApi.Domain.Enums;

namespace TastyRestaurant.WebApi.Mappers;

public static class OrderStatusMapper
{
    public static OrderStatusEnum MapToOrderStatusEnum(this OrderStatusEnumContract status)
    {
        return status switch
        {
            OrderStatusEnumContract.Created => OrderStatusEnum.Created,
            OrderStatusEnumContract.Ready => OrderStatusEnum.Ready,
            OrderStatusEnumContract.Completed => OrderStatusEnum.Completed,
            OrderStatusEnumContract.Cancelled => OrderStatusEnum.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
    
    public static OrderStatusEnumContract MapToOrderStatusEnum(this OrderStatusEnum status)
    {
        return status switch
        {
            OrderStatusEnum.Created => OrderStatusEnumContract.Created,
            OrderStatusEnum.Ready => OrderStatusEnumContract.Ready,
            OrderStatusEnum.Completed => OrderStatusEnumContract.Completed,
            OrderStatusEnum.Cancelled => OrderStatusEnumContract.Completed,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}