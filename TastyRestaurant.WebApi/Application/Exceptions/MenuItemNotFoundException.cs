namespace TastyRestaurant.WebApi.Application.Exceptions;

public class MenuItemNotFoundException : Exception
{
    public MenuItemNotFoundException(Guid menuItemId) : base($"Menu item not found for id: {menuItemId}")
    {
    }
}