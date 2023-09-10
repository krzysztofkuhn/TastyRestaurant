using TastyRestaurant.WebApi.Application.MenuItems.Filters;
using TastyRestaurant.WebApi.Contracts.V1.Requests;

namespace TastyRestaurant.WebApi.Mappers;

public static class GetAllMenuItemsFilterMapper
{
    public static GetAllMenuItemsFilter MapToGetAllMenuItemsFilter(this GetAllMenuItemsFilterRequest request)
    {
        var result = new GetAllMenuItemsFilter(
            request.SearchNamePhrase,
            request.Category,
            request.PriceFrom,
            request.PriceTo);

        return result;
    }
}