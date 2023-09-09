namespace TastyRestaurant.WebApi.Application.Filters;

public record PaginationFilter(uint? PageSize, uint? PageNumber);