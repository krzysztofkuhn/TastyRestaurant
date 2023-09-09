using TastyRestaurant.WebApi.Application.Filters;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Application.Services;

public interface IOrdersService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync(GetAllOrdersFilter filter, PaginationFilter pagination);
    Task<Order> GetOrderByIdAsync(uint orderId);
}