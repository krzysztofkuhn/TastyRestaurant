using TastyRestaurant.WebApi.Domain.Enums;

namespace TastyRestaurant.WebApi.Application.Filters;

public record GetAllOrdersFilter(
    OrderStatusEnum? Status,
    decimal? PriceFrom,
    decimal? PriceTo,
    int? UserId,
    DateTime? CreationDateFrom,
    DateTime? CreationDateTo,
    DateTime? UpdateDateFrom,
    DateTime? UpdateDateTo
);