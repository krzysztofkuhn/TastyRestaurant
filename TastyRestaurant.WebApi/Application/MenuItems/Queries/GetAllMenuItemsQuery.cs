using MediatR;
using TastyRestaurant.WebApi.Application.Common.Filters;
using TastyRestaurant.WebApi.Application.MenuItems.Filters;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Application.MenuItems.Queries;

public record GetAllMenuItemsQuery(GetAllMenuItemsFilter? Filter = null, PaginationFilter? Pagination = null) : IRequest<IEnumerable<MenuItem>>;