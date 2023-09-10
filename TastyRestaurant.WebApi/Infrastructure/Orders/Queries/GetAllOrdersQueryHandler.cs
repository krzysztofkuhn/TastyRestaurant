using MediatR;
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
        throw new NotImplementedException();
    }
}