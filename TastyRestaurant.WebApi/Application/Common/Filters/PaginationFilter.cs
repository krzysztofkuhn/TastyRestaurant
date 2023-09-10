namespace TastyRestaurant.WebApi.Application.Common.Filters;

public record PaginationFilter(int? PageSize, int? PageNumber);