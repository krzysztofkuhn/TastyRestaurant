namespace TastyRestaurant.WebApi.Domain.Entities;

public class OrderItem
{
    public MenuItem MenuItem { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}