namespace TastyRestaurant.WebApi.Domain.Abstract;

public abstract class AggregateRoot<TId> : Entity<TId>
{
    // empty constructor for EF purposes
    protected AggregateRoot() { }

    protected AggregateRoot(TId id) : base(id)
    {
        this.Id = id;
    }
}