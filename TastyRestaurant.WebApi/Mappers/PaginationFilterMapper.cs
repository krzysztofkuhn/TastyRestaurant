using TastyRestaurant.WebApi.Application.Filters;
using TastyRestaurant.WebApi.Contracts.V1.Requests;

namespace TastyRestaurant.WebApi.Mappers;

public static class PaginationFilterMapper
{
    public static PaginationFilter MapToPaginationFilter(this PaginationFilterRequest request)
    {
        var result = new PaginationFilter(request.PageSize, request.PageNumber);
        return result;
    }
}