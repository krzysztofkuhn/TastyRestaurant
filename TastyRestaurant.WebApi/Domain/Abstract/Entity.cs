namespace TastyRestaurant.WebApi.Domain.Abstract;

public abstract class Entity<TId>
{
    // empty constructor for EF purposes
    protected Entity() { }

    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        this.Id = id;
    }

    public override bool Equals(object entity)
    {
        var areEqual = (entity is Entity<TId> otherEntity && this.Equals(otherEntity));
        return areEqual;
    }

    protected bool Equals(Entity<TId> other)
    {
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}