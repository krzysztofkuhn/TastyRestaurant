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
        var query = _dbContext.MenuItems.AsQueryable();
        if (request.Filter is not null)
        {
            if (request.Filter.Category is not null)
            {
                var category = request.Filter.Category.ToLower();
                query = query.Where(x => request.Filter.Category.ToLower() == category);
            }

            if (request.Filter.PriceFrom is not null)
                query = query.Where(x => request.Filter.PriceFrom.Value <= x.Price);

            if (request.Filter.PriceTo is not null)
                query = query.Where(x => x.Price <= request.Filter.PriceTo.Value);

            if (request.Filter.SearchNamePhrase is not null)
            {
                var searchPhrase = request.Filter.SearchNamePhrase.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(searchPhrase));
            }
        }

        query = query.OrderBy(x => x.Category.Id).ThenBy(x => x.Name);

        if (request.Pagination != null)
        {
            var pageNumber = request.Pagination.PageNumber;
            var pageSize = request.Pagination.PageSize;

            if (pageNumber.HasValue || pageSize.HasValue)
            {
                if (pageNumber is null || request.Pagination.PageSize is null)
                    throw new ArgumentNullException("Both PageNumber and PageSize required when passing pagination params");

                if (pageNumber == 0)
                    throw new ArgumentOutOfRangeException("PageNumber must be greater than 0.");

                if (request.Pagination.PageSize == 0)
                    throw new ArgumentOutOfRangeException("PageSize must be greater than 0.");

                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
        }

        var result = await query.Include(x => x.Category).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}