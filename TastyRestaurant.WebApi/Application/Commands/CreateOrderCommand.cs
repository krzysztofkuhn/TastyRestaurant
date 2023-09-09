using MediatR;
using TastyRestaurant.WebApi.Application.Models;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Application.Commands;

public record CreateOrderCommand(int UserId, IEnumerable<OrderItemModel> OrderItems) : IRequest<Order>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
{
    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        return new Order
        {
            Id = 10
        };
    }
}