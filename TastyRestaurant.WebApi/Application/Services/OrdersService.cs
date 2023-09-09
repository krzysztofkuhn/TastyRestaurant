using TastyRestaurant.WebApi.Application.Filters;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Enums;

namespace TastyRestaurant.WebApi.Application.Services;

public class OrdersService : IOrdersService
{
    public async Task<IEnumerable<Order>> GetAllOrdersAsync(GetAllOrdersFilter filter, PaginationFilter pagination)
    {
        throw new NotImplementedException();
    }

    public async Task<Order> GetOrderByIdAsync(uint orderId)
    {
        throw new NotImplementedException();
    }
}