using TastyRestaurant.WebApi.Domain.Enums;

namespace TastyRestaurant.WebApi.Domain.Entities;

public class Order
{
    public OrderStatusEnum Status { get; set; }
    public decimal Price { get; set; }
    public Guest User { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set;}
}