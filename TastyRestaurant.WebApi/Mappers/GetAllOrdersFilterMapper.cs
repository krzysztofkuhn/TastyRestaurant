using TastyRestaurant.WebApi.Application.Filters;
using TastyRestaurant.WebApi.Contracts.V1.Requests;

namespace TastyRestaurant.WebApi.Mappers;

public static class GetAllOrdersFilterMapper
{
    public static GetAllOrdersFilter MapToGetAllOrdersFilter(this GetAllOrdersFilterRequest request)
    {
        var result = new GetAllOrdersFilter(
            request.Status?.MapToOrderStatusEnum(),
            request.PriceFrom,
            request.PriceTo,
            request.UserId,
            request.CreationDateFrom,
            request.CreationDateTo,
            request.UpdateDateFrom,
            request.UpdateDateTo);

        return result;
    }
}