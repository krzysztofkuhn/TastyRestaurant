using MediatR;
using TastyRestaurant.WebApi.Application.Exceptions;
using TastyRestaurant.WebApi.Application.Models;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Enums;
using TastyRestaurant.WebApi.Domain.Exceptions;
using TastyRestaurant.WebApi.Domain.Repositories;

namespace TastyRestaurant.WebApi.Application.Commands;

public record UpdateOrderCommand(Guid OrderId, OrderStatusEnum Status, IEnumerable<OrderItemModel> OrderItems) : IRequest<Order>;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository)
    {
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        // get order
        var order = await _orderRepository.GetAsync(request.OrderId);
        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        // update order items
        if (OrderItemsChanged(order.OrderItems, request.OrderItems))
        {
            var orderItems = await GetOrderItems(request.OrderItems);
            order.UpdateOrderItems(orderItems);
        }

        // if target status is different than current status
        // change order status
        var targetStatus = request.Status;
        if (order.Status != targetStatus)
        {
            ApplyStatusChange(targetStatus, order);
        }

        // save changes
        await _orderRepository.UpdateAsync(order);

        return order;
    }

    private bool OrderItemsChanged(IReadOnlyList<OrderItem> previousOrderItems, IEnumerable<OrderItemModel> newOrderItems)
    {
        if (previousOrderItems.Count != newOrderItems.Count()) return true;

        var previousList = previousOrderItems.Select(x => new OrderItemModel(x.MenuItem.Id, x.Quantity)).OrderBy(x => x.MenuItemId).ToArray();
        var newList = newOrderItems.OrderBy(x => x.MenuItemId).ToArray();

        for (int i = 0; i < previousList.Length; i++)
        {
            if (previousList[i] != newList[i]) 
                return true;
        }

        return false;
    }

    private static void ApplyStatusChange(OrderStatusEnum targetStatus, Order order)
    {
        switch (targetStatus)
        {
            case OrderStatusEnum.Created:
                throw new InvalidStatusChangeException($"Cannot change status '{order.Status}' to 'created'");
            case OrderStatusEnum.Ready:
                order.Ready();
                break;
            case OrderStatusEnum.Completed:
                order.Complete();
                break;
            case OrderStatusEnum.Cancelled:
                order.Cancel();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task<List<OrderItem>> GetOrderItems(IEnumerable<OrderItemModel> requestOrderItems)
    {
        var allMenuItems = await _menuItemRepository.GetAllAsync();
        var orderItems = new List<OrderItem>();
        foreach (var requestOrderItem in requestOrderItems)
        {
            // get menu item based on menu item id
            var menuItem = allMenuItems.FirstOrDefault(x => x.Id == requestOrderItem.MenuItemId);
            // if not exists - throw not found exception
            if (menuItem is null)
            {
                throw new MenuItemNotFoundException(requestOrderItem.MenuItemId);
            }

            orderItems.Add(OrderItem.Create(menuItem, requestOrderItem.Quantity));
        }

        return orderItems;
    }
}