using MediatR;
using TastyRestaurant.WebApi.Application.Common.Filters;
using TastyRestaurant.WebApi.Application.Orders.Filters;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Application.Orders.Queries;

public record GetAllOrdersQuery(GetAllOrdersFilter? Filter, PaginationFilter? Pagination) : IRequest<IEnumerable<Order>>;