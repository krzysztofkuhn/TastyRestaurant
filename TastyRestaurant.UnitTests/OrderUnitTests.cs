using TastyRestaurant.UnitTests.SampleData;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Exceptions;

namespace TastyRestaurant.UnitTests;

public class OrderUnitTests
{
    [Fact]
    public void Create_Creates_Order_If_Correct_Ids_Provided_And_Order_Is_Not_Empty()
    {
        // arrange
        Guid orderId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        OrderItem beefTartareOrderItem = OrderItem.Create(MenuItemSampleData.BeefTartare, 1);
        OrderItem steakOrderItem = OrderItem.Create(MenuItemSampleData.Steak, 1);
        OrderItem twoBeersOrderItem = OrderItem.Create(MenuItemSampleData.Beer, 2);
        OrderItem[] initialOrderItems = { beefTartareOrderItem, steakOrderItem, twoBeersOrderItem };
        decimal expectedOrderTotal = 156.0m;
        DateTime testStart = DateTime.Now;

        // act - system under test => order
        Order sut = Order.CreateOrder(orderId, userId, initialOrderItems);

        // assert
        Assert.Equal(orderId, sut.Id);
        Assert.Equal(userId, sut.UserId);
        Assert.True(testStart < sut.CreationDate);
        Assert.Equal(sut.CreationDate, sut.UpdateDate);
        Assert.Equal(3, sut.OrderItems.Count);
        Assert.Contains(beefTartareOrderItem, sut.OrderItems);
        Assert.Contains(steakOrderItem, sut.OrderItems);
        Assert.Contains(twoBeersOrderItem, sut.OrderItems);
        Assert.Equal(expectedOrderTotal, sut.Price);
    }

    [Theory]
    [MemberData(nameof(Get_Create_InvalidGuidTestData))]
    public void Create_Throws_GuidRequiredException_If_Empty_Guid_Provided(Guid orderId, Guid userId, IEnumerable<OrderItem> initialOrderItems)
    {
        Assert.Throws<GuidRequiredException>(() => Order.CreateOrder(orderId, userId, initialOrderItems));
    }

    public static IEnumerable<object[]> Get_Create_InvalidGuidTestData()
    {
        var data = new[]
        {
            new object[] { Guid.Empty, Guid.NewGuid(), new List<OrderItem>() },
            new object[] { Guid.NewGuid(), Guid.Empty, new List<OrderItem>() },
        };

        return data;
    }

    [Theory]
    [MemberData(nameof(Get_Create_EmptyOrderItemsCollectionTestData))]
    public void Create_Throws_CannotCreateEmptyOrderException_If_Empty_OrderItems_Provided(Guid orderId, Guid userId, IEnumerable<OrderItem> initialOrderItems)
    {
        Assert.Throws<CannotCreateEmptyOrderException>(() => Order.CreateOrder(orderId, userId, initialOrderItems));
    }
    
    public static IEnumerable<object[]> Get_Create_EmptyOrderItemsCollectionTestData()
    {
        var data = new[]
        {
            new object[] { Guid.NewGuid(), Guid.NewGuid(), null },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), new List<OrderItem>() }
        };

        return data;
    }


}