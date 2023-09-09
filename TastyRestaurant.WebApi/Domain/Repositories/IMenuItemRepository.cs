using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Domain.Repositories;

public interface IMenuItemRepository
{
    public Task<IEnumerable<MenuItem>> GetAllAsync();
}