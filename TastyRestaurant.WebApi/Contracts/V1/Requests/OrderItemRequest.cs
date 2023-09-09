namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public class OrderItemRequest
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}