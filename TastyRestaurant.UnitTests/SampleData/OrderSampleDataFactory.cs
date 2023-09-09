using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Enums;

namespace TastyRestaurant.UnitTests.SampleData;

internal class OrderSampleDataFactory
{
    public static Order GetOrderWithStatus(OrderStatusEnum status)
    {
        var sampleBeefOrder = Order.Create(Guid.NewGuid(), Guid.NewGuid(), new[] { OrderItem.Create(MenuItemSampleData.BeefTartare, 1) });

        switch (status)
        {
            case OrderStatusEnum.Created:
                return sampleBeefOrder;
            case OrderStatusEnum.Ready:
                sampleBeefOrder.Ready();
                return sampleBeefOrder;
            case OrderStatusEnum.Completed:
                sampleBeefOrder.Ready();
                sampleBeefOrder.Complete();
                return sampleBeefOrder;
            case OrderStatusEnum.Cancelled:
                sampleBeefOrder.Cancel();
                return sampleBeefOrder;
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
    }
}