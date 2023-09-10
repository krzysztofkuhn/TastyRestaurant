using MediatR;
using TastyRestaurant.WebApi.Application.MenuItems.Queries;
using TastyRestaurant.WebApi.Application.Orders.Exceptions;
using TastyRestaurant.WebApi.Application.Orders.Models;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Repositories;

namespace TastyRestaurant.WebApi.Application.Orders.Commands;

public record CreateOrderCommand(Guid UserId, IEnumerable<OrderItemModel> OrderItems) : IRequest<Order>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISender _mediator;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUserRepository userRepository, ISender mediator)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _mediator = mediator;
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
        var allMenuItems = await _mediator.Send(new GetAllMenuItemsQuery());
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