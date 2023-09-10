using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Repositories;
using TastyRestaurant.WebApi.Infrastructure.Persistence;

namespace TastyRestaurant.WebApi.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Order?> GetAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Order order)
    {
        throw new NotImplementedException();
    }
}