namespace TastyRestaurant.WebApi.Application.Common.Filters;

public record PaginationFilter(uint? PageSize, uint? PageNumber);