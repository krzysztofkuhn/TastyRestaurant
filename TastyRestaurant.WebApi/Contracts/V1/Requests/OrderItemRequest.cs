namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public class OrderItemRequest
{
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }
}