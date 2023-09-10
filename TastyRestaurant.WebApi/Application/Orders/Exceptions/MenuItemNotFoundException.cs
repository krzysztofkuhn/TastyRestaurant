namespace TastyRestaurant.WebApi.Application.Orders.Exceptions;

public class MenuItemNotFoundException : Exception
{
    public MenuItemNotFoundException(Guid menuItemId) : base($"Menu item not found for id: {menuItemId}")
    {
    }
}