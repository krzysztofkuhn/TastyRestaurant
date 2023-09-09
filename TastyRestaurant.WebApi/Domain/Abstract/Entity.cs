namespace TastyRestaurant.WebApi.Domain.Abstract
{
    public abstract class Entity<TId>
    {
        public TId Id { get; init; }

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
}