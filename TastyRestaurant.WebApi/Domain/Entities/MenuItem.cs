using TastyRestaurant.WebApi.Domain.Abstract;

namespace TastyRestaurant.WebApi.Domain.Entities;

public sealed class MenuItem : Entity<Guid>
{
    public string Name { get; private set; }
    public MenuItemCategory Category { get; private set; }
    public decimal Price { get; private set; }
    public string Image { get; private set; }

    private MenuItem(Guid id, string name, MenuItemCategory category, decimal price, string image) : base(id)
    {
        Name = name;
        Category = category;
        Price = price;
        Image = image;
    }

    public static MenuItem Create(Guid id, string name, MenuItemCategory category, decimal price, string image)
    {
        return new MenuItem(id, name, category, price, image);
    }
}