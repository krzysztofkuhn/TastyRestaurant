namespace TastyRestaurant.WebApi.Application.Models;

public record OrderItemModel(Guid MenuItemId, int Quantity);