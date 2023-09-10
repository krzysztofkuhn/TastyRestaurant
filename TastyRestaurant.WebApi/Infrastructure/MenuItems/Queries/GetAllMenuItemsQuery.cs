using MediatR;
using Microsoft.EntityFrameworkCore;
using TastyRestaurant.WebApi.Application.MenuItems.Queries;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Infrastructure.Persistence;

namespace TastyRestaurant.WebApi.Infrastructure.MenuItems.Queries;

public class GetAllMenuItemsQueryHandler : IRequestHandler<GetAllMenuItemsQuery, IEnumerable<MenuItem>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllMenuItemsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<MenuItem>> Handle(GetAllMenuItemsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.MenuItems.ToListAsync(cancellationToken);
    }
}