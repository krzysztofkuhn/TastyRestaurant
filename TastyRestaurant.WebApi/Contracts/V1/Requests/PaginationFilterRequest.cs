using Microsoft.AspNetCore.Mvc;

namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public record PaginationFilterRequest
{
    [FromQuery(Name = "pageSize")]
    public uint? PageSize { get; init; }
    [FromQuery(Name = "pageNumber")]
    public uint? PageNumber { get; init; }
}