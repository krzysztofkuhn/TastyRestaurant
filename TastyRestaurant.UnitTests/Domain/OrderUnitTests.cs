using TastyRestaurant.UnitTests.SampleData;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Enums;
using TastyRestaurant.WebApi.Domain.Exceptions;

namespace TastyRestaurant.UnitTests.Domain;

public class OrderUnitTests
{
    #region Create

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
        Order sut = Order.Create(orderId, userId, initialOrderItems);

        // assert
        Assert.Equal(orderId, sut.Id);
        Assert.Equal(userId, sut.UserId);
        Assert.True(testStart < sut.CreationDate);
        Assert.Equal(sut.CreationDate, sut.UpdateDate);
        Assert.Equal(OrderStatusEnum.Created, sut.Status);
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
        Assert.Throws<GuidRequiredException>(() => Order.Create(orderId, userId, initialOrderItems));
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
        Assert.Throws<CannotCreateEmptyOrderException>(() => Order.Create(orderId, userId, initialOrderItems));
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
    #endregion

    #region UpdateOrderItems
    [Fact]
    public void UpdateOrderItems_Updates_Order_Items_List_When_Status_Created()
    {
        // arrange
        var sut = Order.Create(Guid.NewGuid(), Guid.NewGuid(), new[] { OrderItem.Create(MenuItemSampleData.BeefTartare, 1) });
        var originalUpdateDate = sut.UpdateDate;
        var burger = OrderItem.Create(MenuItemSampleData.Burger, 1);
        var newOrderItems = new[] { burger };

        // act
        sut.UpdateOrderItems(newOrderItems);

        // assert
        Assert.Equal(1, sut.OrderItems.Count);
        Assert.Contains(burger, sut.OrderItems);
        Assert.True(sut.UpdateDate > originalUpdateDate);
    }

    [Fact]
    public void UpdateOrderItems_Combines_Duplicated_Order_Items()
    {
        // arrange
        var sut = Order.Create(Guid.NewGuid(), Guid.NewGuid(), new[] { OrderItem.Create(MenuItemSampleData.BeefTartare, 1) });
        var burger1 = OrderItem.Create(MenuItemSampleData.Burger, 1);
        var beefTartare = OrderItem.Create(MenuItemSampleData.BeefTartare, 1);
        var burger3 = OrderItem.Create(MenuItemSampleData.Burger, 3);
        var newOrderItems = new[] { burger1, beefTartare, burger3 };
        var expectedBurgerItem = OrderItem.Create(MenuItemSampleData.Burger, 4);

        // act
        sut.UpdateOrderItems(newOrderItems);

        // assert
        Assert.Equal(2, sut.OrderItems.Count); // we expect 2 lines: 1. for 4 x burger, 2. for 1 x beef tartare
        Assert.Contains(beefTartare, sut.OrderItems);
        Assert.Contains(expectedBurgerItem, sut.OrderItems);
    }

    [Theory]
    [MemberData(nameof(Get_UpdateOrderItems_OrdersWithNonCreatedStatuses))]
    public void UpdateOrderItems_Throws_OrderAlreadyProcessedException_When_Status_Other_Than_Created(Order sut)
    {
        // arrange
        var newOrderItems = new[] { OrderItem.Create(MenuItemSampleData.Burger, 1) };

        // act/assert
        Assert.Throws<OrderAlreadyProcessedException>(() => sut.UpdateOrderItems(newOrderItems));
    }

    public static IEnumerable<object[]> Get_UpdateOrderItems_OrdersWithNonCreatedStatuses()
    {
        var data = new[]
        {
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Cancelled) },
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Completed) },
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Ready) }
        };

        return data;
    }
    #endregion

    #region Ready
    [Fact]
    public void Ready_Sets_Status_Ready_When_Status_Created()
    {
        // arrange
        var sut = Order.Create(Guid.NewGuid(), Guid.NewGuid(), new[] { OrderItem.Create(MenuItemSampleData.BeefTartare, 1) });
        var testStartDate = DateTime.Now;

        // act
        sut.Ready();

        // assert
        Assert.Equal(OrderStatusEnum.Ready, sut.Status);
        Assert.True(sut.UpdateDate > testStartDate);
    }

    [Theory]
    [MemberData(nameof(Get_Ready_OrdersWithNonCreatedStatuses))]
    public void Ready_Throws_InvalidStatusChangeException_When_Status_Other_Than_Created(Order sut)
    {
        // act/assert
        Assert.Throws<InvalidStatusChangeException>(() => sut.Ready());
    }

    public static IEnumerable<object[]> Get_Ready_OrdersWithNonCreatedStatuses()
    {
        var data = new[]
        {
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Cancelled) },
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Completed) },
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Ready) }
        };

        return data;
    }
    #endregion

    #region Complete
    [Fact]
    public void Complete_Sets_Status_Completed_When_Status_Ready()
    {
        // arrange
        var sut = Order.Create(Guid.NewGuid(), Guid.NewGuid(), new[] { OrderItem.Create(MenuItemSampleData.BeefTartare, 1) });
        sut.Ready();
        var testStartDate = DateTime.Now;

        // act
        sut.Complete();

        // assert
        Assert.Equal(OrderStatusEnum.Completed, sut.Status);
        Assert.True(sut.UpdateDate > testStartDate);
    }

    [Theory]
    [MemberData(nameof(Get_Complete_OrdersWithNonReadyStatuses))]
    public void Complete_Throws_InvalidStatusChangeException_When_Status_Other_Than_Ready(Order sut)
    {
        // act/assert
        Assert.Throws<InvalidStatusChangeException>(() => sut.Complete());
    }

    public static IEnumerable<object[]> Get_Complete_OrdersWithNonReadyStatuses()
    {
        var data = new[]
        {
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Created) },
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Cancelled) },
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Completed) },
        };

        return data;
    }
    #endregion

    #region Cancel
    [Theory]
    [MemberData(nameof(Get_Cancel_OrdersWithCreatedOrReadyStatuses))]
    public void Cancel_Sets_Status_Cancelled_When_Status_Ready_Or_Created(Order sut)
    {
        // arrange
        var testStartDate = DateTime.Now;

        // act
        sut.Cancel();

        // assert
        Assert.Equal(OrderStatusEnum.Cancelled, sut.Status);
        Assert.True(sut.UpdateDate > testStartDate);
    }

    public static IEnumerable<object[]> Get_Cancel_OrdersWithCreatedOrReadyStatuses()
    {
        var data = new[]
        {
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Created) },
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Ready) },
        };

        return data;
    }

    [Theory]
    [MemberData(nameof(Get_Cancel_OrdersWithCompletedOrCancelledStatuses))]
    public void Cancel_Throws_InvalidStatusChangeException_When_Status_Not_Created_Or_Ready(Order sut)
    {
        // act/assert
        Assert.Throws<InvalidStatusChangeException>(() => sut.Cancel());
    }

    public static IEnumerable<object[]> Get_Cancel_OrdersWithCompletedOrCancelledStatuses()
    {
        var data = new[]
        {
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Completed) },
            new object[] { OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Cancelled) },
        };

        return data;
    }
    #endregion
}