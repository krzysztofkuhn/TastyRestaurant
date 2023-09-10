using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Domain.Repositories;
using TastyRestaurant.WebApi.Infrastructure.Persistence;

namespace TastyRestaurant.WebApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicationUser?> GetAsync(Guid userId)
    {
        return await _dbContext.Users.FindAsync(userId.ToString());
    }
}