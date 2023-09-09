using TastyRestaurant.WebApi.Domain.Abstract;

namespace TastyRestaurant.WebApi.Domain.Entities;

public sealed class MenuItemCategory : Entity<Guid>
{
    public string Name { get; private set; }

    private MenuItemCategory(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static MenuItemCategory Create(Guid id, string name)
    {
        return new MenuItemCategory(id, name);
    }
}