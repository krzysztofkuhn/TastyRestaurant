namespace TastyRestaurant.WebApi.Domain.Abstract
{
    public abstract class AggregateRoot<TId> : Entity<TId>
    {
        protected AggregateRoot(TId id) : base(id)
        {
            this.Id = id;
        }
    }
}