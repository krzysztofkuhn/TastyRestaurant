using TastyRestaurant.WebApi.Domain.Abstract;

namespace TastyRestaurant.WebApi.Domain.Entities;

// order item is a value object - it cannot live without order context
public sealed class OrderItem : ValueObject
{
    // empty constructor for EF purposes
    private OrderItem() {}
    // artificial Id - for EF purposes
    public Guid Id { get; private set; }
    public MenuItem MenuItem { get; private set; }

    public int Quantity { get; private set; }

    public decimal TotalPrice => MenuItem.Price * Quantity;

    private OrderItem(MenuItem menuItem, int quantity)
    {
        Id = Guid.NewGuid();
        MenuItem = menuItem;
        Quantity = quantity;

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity has to be greater than 0.");
    }

    public static OrderItem Create(MenuItem menuItem, int quantity)
    {
        return new OrderItem(menuItem, quantity);
    }

    // value objects are compared by properties; this metod returns properties to compare when comparing with other OrderItem object
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MenuItem;
        yield return Quantity;
    }
}