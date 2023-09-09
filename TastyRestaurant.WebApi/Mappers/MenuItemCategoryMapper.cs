using TastyRestaurant.WebApi.Contracts.V1.Responses;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Mappers;

public static class MenuItemCategoryMapper
{
    public static MenuItemCategoryResponse MapToMenuItemCategoryResponse(this MenuItemCategory menuItemCategory)
    {
        var result = new MenuItemCategoryResponse(menuItemCategory.Id, menuItemCategory.Name);
        return result;
    }
}