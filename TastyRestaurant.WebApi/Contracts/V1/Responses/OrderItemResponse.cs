namespace TastyRestaurant.WebApi.Contracts.V1.Responses;

public record OrderItemResponse(MenuItemResponse MenuItem, int Quantity, decimal TotalPrice);