namespace TastyRestaurant.WebApi.Domain.Exceptions;

public class GuidRequiredException : Exception
{
    public GuidRequiredException(string message) : base(message) { }
}