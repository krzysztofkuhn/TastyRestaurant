using MediatR;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Application.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Order>;