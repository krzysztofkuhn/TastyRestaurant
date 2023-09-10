﻿using MediatR;
using TastyRestaurant.WebApi.Application.Orders.Exceptions;
using TastyRestaurant.WebApi.Application.Orders.Queries;
using TastyRestaurant.WebApi.Domain.Entities;
using TastyRestaurant.WebApi.Infrastructure.Persistence;

namespace TastyRestaurant.WebApi.Infrastructure.Orders.Queries;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
{
    private readonly ApplicationDbContext _dbContext;

    public GetOrderByIdQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FindAsync(request.OrderId.ToString(), cancellationToken);
        if (order == null) 
            throw new OrderNotFoundException(request.OrderId);

        return order;
    }
}