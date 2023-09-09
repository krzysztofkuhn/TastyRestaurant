using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Guest?> GetAsync(Guid userId);
        Task AddAsync(Guest order);
        Task UpdateAsync(Guest order);
        Task DeleteAsync(Guest order);
    }
}