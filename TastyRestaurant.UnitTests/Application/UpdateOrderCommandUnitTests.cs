using MediatR;
using NSubstitute;
using TastyRestaurant.UnitTests.SampleData;
using TastyRestaurant.WebApi.Application.MenuItems.Queries;
using TastyRestaurant.WebApi.Application.Orders.Commands;
using TastyRestaurant.WebApi.Application.Orders.Exceptions;
using TastyRestaurant.WebApi.Application.Orders.Models;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Enums;
using TastyRestaurant.WebApi.Domain.Repositories;

namespace TastyRestaurant.UnitTests.Application;

public class UpdateOrderCommandUnitTests
{
    #region ARRANGE

    // system under test
    private readonly UpdateOrderCommandHandler _sut;
    private readonly IOrderRepository _orderRepository;
    private readonly ISender _mediator;

    public UpdateOrderCommandUnitTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _mediator = Substitute.For<ISender>();
        // make menu item repo return all menu items - by default
        _mediator.Send(Arg.Any<GetAllMenuItemsQuery>()).Returns(MenuItemSampleData.All);

        _sut = new UpdateOrderCommandHandler(_orderRepository, _mediator);
    }
    #endregion

    [Fact]
    public async Task Handle_Calls_Update_Repo_On_Update_Command()
    {
        // arrange
        // get base order
        Order initialOrder = OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Created);
        // make order repo return initial order
        _orderRepository.GetAsync(initialOrder.Id).Returns(initialOrder);
        // setup command parameters
        IEnumerable<OrderItemModel> originalOrderItems = initialOrder.OrderItems.Select(x => new OrderItemModel(x.MenuItem.Id, x.Quantity));
        OrderItem expectedOrderItem = OrderItem.Create(MenuItemSampleData.BeefTartare, 1);
        OrderStatusEnum newOrderStatusEnum = OrderStatusEnum.Ready;

        // make order repo save updated order
        Order? updatedRepoOrder = default;
        _orderRepository.When(x => x.UpdateAsync(Arg.Any<Order>())).Do(args => updatedRepoOrder = (Order)args[0]);

        // update command
        UpdateOrderCommand command = new UpdateOrderCommand(initialOrder.Id, newOrderStatusEnum, originalOrderItems);

        // act
        Order updatedOrder = await _sut.Handle(command, CancellationToken.None);

        // assert
        await _orderRepository.Received(1).UpdateAsync(Arg.Any<Order>()); //check if order repo add async method has been called once
        Assert.Equal(1, updatedOrder.OrderItems.Count);
        Assert.Contains(expectedOrderItem, updatedOrder.OrderItems);
        Assert.Equal(updatedOrder, updatedRepoOrder);
    }

    [Fact]
    public async Task Handle_Sets_Order_Ready_On_Status_Change_Ready_Command()
    {
        // arrange
        // get base order
        Order initialOrder = OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Created);
        // make order repo return initial order
        _orderRepository.GetAsync(initialOrder.Id).Returns(initialOrder);
        // setup command parameters
        IEnumerable<OrderItemModel> originalOrderItems = initialOrder.OrderItems.Select(x => new OrderItemModel(x.MenuItem.Id, x.Quantity));
        OrderStatusEnum newOrderStatusEnum = OrderStatusEnum.Ready;

        // make order repo save updated order
        Order? updatedRepoOrder = default;
        _orderRepository.When(x => x.UpdateAsync(Arg.Any<Order>())).Do(args => updatedRepoOrder = (Order)args[0]);

        // update command
        UpdateOrderCommand command = new UpdateOrderCommand(initialOrder.Id, newOrderStatusEnum, originalOrderItems);

        // act
        Order updatedOrder = await _sut.Handle(command, CancellationToken.None);

        // assert
        Assert.Equal(newOrderStatusEnum, updatedOrder.Status);
    }

    [Fact]
    public async Task Handle_Sets_Order_Complete_On_Status_Change_Completed_Command()
    {
        // arrange
        // get base order
        Order initialOrder = OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Ready);
        // make order repo return initial order
        _orderRepository.GetAsync(initialOrder.Id).Returns(initialOrder);
        // setup command parameters
        IEnumerable<OrderItemModel> originalOrderItems = initialOrder.OrderItems.Select(x => new OrderItemModel(x.MenuItem.Id, x.Quantity));
        OrderStatusEnum newOrderStatusEnum = OrderStatusEnum.Completed;

        // update command
        UpdateOrderCommand command = new UpdateOrderCommand(initialOrder.Id, newOrderStatusEnum, originalOrderItems);

        // act
        Order updatedOrder = await _sut.Handle(command, CancellationToken.None);

        // assert
        Assert.Equal(newOrderStatusEnum, updatedOrder.Status);
    }

    [Fact]
    public async Task Handle_Sets_Order_Cancelled_On_Status_Change_Cancelled_Command()
    {
        // arrange
        // get base order
        Order initialOrder = OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Created);
        // make order repo return initial order
        _orderRepository.GetAsync(initialOrder.Id).Returns(initialOrder);
        // setup command parameters
        IEnumerable<OrderItemModel> originalOrderItems = initialOrder.OrderItems.Select(x => new OrderItemModel(x.MenuItem.Id, x.Quantity));
        OrderStatusEnum newOrderStatusEnum = OrderStatusEnum.Cancelled;

        // update command
        UpdateOrderCommand command = new UpdateOrderCommand(initialOrder.Id, newOrderStatusEnum, originalOrderItems);

        // act
        Order updatedOrder = await _sut.Handle(command, CancellationToken.None);

        // assert
        Assert.Equal(newOrderStatusEnum, updatedOrder.Status);
    }

    [Fact]
    public async Task Handle_Adds_Order_Item_When_Update_Order_Items_Collection_Changed()
    {
        // arrange
        // get base order
        Order initialOrder = OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Created);
        OrderStatusEnum initialStatus = initialOrder.Status;
        // make order repo return initial order
        _orderRepository.GetAsync(initialOrder.Id).Returns(initialOrder);
        // setup command parameters
        var orderItemModels = new List<OrderItemModel>
        {
            new (MenuItemSampleData.BeefTartare.Id, 1),
            new (MenuItemSampleData.FrenchOnionSoup.Id, 3) // new item
        };

        // update command
        UpdateOrderCommand command = new UpdateOrderCommand(initialOrder.Id, initialStatus, orderItemModels);

        // act
        Order updatedOrder = await _sut.Handle(command, CancellationToken.None);

        // assert
        Assert.Equal(2, updatedOrder.OrderItems.Count);
        Assert.Contains(OrderItem.Create(MenuItemSampleData.BeefTartare, 1), updatedOrder.OrderItems);
        Assert.Contains(OrderItem.Create(MenuItemSampleData.FrenchOnionSoup, 3), updatedOrder.OrderItems);
        Assert.Equal(initialStatus, updatedOrder.Status);
    }

    [Fact]
    public async Task Handle_Throws_OrderNotFoundException_When_Invalid_OrderId()
    {
        // arrange
        //create command
        var invalidOrderGuid = Guid.NewGuid();
        UpdateOrderCommand command = new UpdateOrderCommand(invalidOrderGuid, OrderStatusEnum.Created, Enumerable.Empty<OrderItemModel>());

        // act/assert
        await Assert.ThrowsAsync<OrderNotFoundException>(() => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Throws_MenuItemNotFoundException_When_Invalid_MenuItemId()
    {
        // arrange
        // get base order
        Order initialOrder = OrderSampleDataFactory.GetOrderWithStatus(OrderStatusEnum.Created);
        // make order repo return initial order
        _orderRepository.GetAsync(initialOrder.Id).Returns(initialOrder);

        //items to add
        var notExistingMenuItemGuid = Guid.NewGuid();
        var orderItemModels = new OrderItemModel[] { new(notExistingMenuItemGuid, 1) };
        //create command
        UpdateOrderCommand command = new UpdateOrderCommand(initialOrder.Id, initialOrder.Status, orderItemModels);

        // act/assert
        await Assert.ThrowsAsync<MenuItemNotFoundException>(() => _sut.Handle(command, CancellationToken.None));
    }
}