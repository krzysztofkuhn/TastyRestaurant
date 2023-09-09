namespace TastyRestaurant.WebApi.Domain.Exceptions
{
    public class CannotCreateEmptyOrderException : Exception
    {
        public CannotCreateEmptyOrderException(string message) : base(message) { }
    }
}