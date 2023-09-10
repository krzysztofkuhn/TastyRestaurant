namespace TastyRestaurant.WebApi.Application.Orders.Models;

public record OrderItemModel(Guid MenuItemId, int Quantity);