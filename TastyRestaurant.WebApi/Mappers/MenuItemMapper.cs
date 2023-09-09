using TastyRestaurant.WebApi.Contracts.V1.Responses;
using TastyRestaurant.WebApi.Domain.Entities;

namespace TastyRestaurant.WebApi.Mappers;

public static class MenuItemMapper
{
    public static List<MenuItemResponse> MapToMenuItemResponseList(this IEnumerable<MenuItem> menuItems)
    {
        var result = menuItems
            .Select(x => x.MapToMenuItemResponse())
            .ToList();

        return result;
    }

    public static MenuItemResponse MapToMenuItemResponse(this MenuItem menuItem)
    {
        var result = new MenuItemResponse(
            menuItem.Id,
            menuItem.Name,
            menuItem.Category.MapToMenuItemCategoryResponse(),
            menuItem.Price,
            menuItem.Image);

        return result;
    }
}