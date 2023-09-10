using MediatR;
using Microsoft.EntityFrameworkCore;
using TastyRestaurant.WebApi.Application.Orders.Queries;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Infrastructure.Persistence;

namespace TastyRestaurant.WebApi.Infrastructure.Orders.Queries;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllOrdersQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Orders.AsQueryable();
        if (request.Filter is not null)
        {
            if (request.Filter.UserId is not null)
                query = query.Where(x => x.UserId == request.Filter.UserId);

            if (request.Filter.CreationDateFrom is not null)
                query = query.Where(x => request.Filter.CreationDateFrom.Value <= x.CreationDate);

            if (request.Filter.CreationDateTo is not null)
                query = query.Where(x => x.CreationDate <= request.Filter.CreationDateTo.Value);

            if (request.Filter.UpdateDateFrom is not null)
                query = query.Where(x => request.Filter.UpdateDateFrom.Value <= x.UpdateDate);

            if (request.Filter.UpdateDateTo is not null)
                query = query.Where(x => x.UpdateDate <= request.Filter.UpdateDateTo.Value);

            if (request.Filter.PriceFrom is not null)
                query = query.Where(x => request.Filter.PriceFrom.Value <= x.Price);

            if (request.Filter.PriceTo is not null)
                query = query.Where(x => x.Price <= request.Filter.PriceTo.Value);

            if (request.Filter.Status is not null)
                query = query.Where(x => x.Status == request.Filter.Status.Value);
        }

        query = query.OrderBy(x => x.CreationDate);

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

        var result = await query.ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}