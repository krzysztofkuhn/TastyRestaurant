using MediatR;
using TastyRestaurant.WebApi.Application.Exceptions;
using TastyRestaurant.WebApi.Application.Models;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Repositories;

namespace TastyRestaurant.WebApi.Application.Commands;

public record CreateOrderCommand(Guid UserId, IEnumerable<OrderItemModel> OrderItems) : IRequest<Order>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUserRepository userRepository, IMenuItemRepository menuItemRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // verify if user placing order exists
        var user = await _userRepository.GetAsync(request.UserId);
        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        // convert <MenuItemId, Quantity> pairs to OrderItems
        var orderItems = await GetOrderItems(request.OrderItems);

        // create new order
        var newGuid = Guid.NewGuid();
        var order = Order.Create(newGuid, request.UserId, orderItems);

        // save order to DB
        await _orderRepository.AddAsync(order);

        return order;
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