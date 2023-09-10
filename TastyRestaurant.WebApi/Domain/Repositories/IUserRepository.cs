using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Domain.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetAsync(Guid userId);
}