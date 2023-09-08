namespace TastyRestaurant.WebApi.Domain.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public MenuItemCategory Category { get; set; }
    public decimal Price { get; set; }
    public string Image { get; set; }
}