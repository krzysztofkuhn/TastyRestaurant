using Microsoft.EntityFrameworkCore;
using TastyRestaurant.WebApi.Application.Orders.Exceptions;
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

    public async Task<Order?> GetAsync(Guid orderId)
    {
        var order = await _dbContext.Orders
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.MenuItem)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == orderId);

        if (order == null)
            throw new OrderNotFoundException(orderId);

        return order;
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _dbContext.Update(order);

        var saved = false;
        while (!saved)
        {
            try
            {
                // Attempt to save changes to the database
                await _dbContext.SaveChangesAsync();
                saved = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is OrderItem)
                    {
                        _dbContext.Entry(entry.Entity).State = EntityState.Added;
                    }
                    else
                    {
                        throw new NotSupportedException($"Don't know how to handle concurrency conflicts for {entry.Metadata.Name}");
                    }
                }
            }
        }
    }
}