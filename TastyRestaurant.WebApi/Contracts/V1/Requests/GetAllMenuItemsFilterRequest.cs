namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public record GetAllMenuItemsFilterRequest(
    string? SearchNamePhrase,
    string? Category,
    decimal? PriceFrom,
    decimal? PriceTo
);