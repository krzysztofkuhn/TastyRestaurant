using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetAsync(Guid orderId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }
}