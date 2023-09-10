using Microsoft.AspNetCore.Mvc;

namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public record PaginationFilterRequest
{
    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; init; }
    [FromQuery(Name = "pageNumber")]
    public int? PageNumber { get; init; }
}