using MediatR;
using TastyRestaurant.WebApi.Application.Models;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Enums;

namespace TastyRestaurant.WebApi.Application.Commands;

public record UpdateOrderCommand(uint OrderId, OrderStatusEnum Status, IEnumerable<OrderItemModel> OrderItems) : IRequest<Order>;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
{
    public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        return Order.CreateOrder(Guid.NewGuid(), Guid.NewGuid(), new List<OrderItem>());
    }
}