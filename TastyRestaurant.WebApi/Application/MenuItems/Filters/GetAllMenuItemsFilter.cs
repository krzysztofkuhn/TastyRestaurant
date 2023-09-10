namespace TastyRestaurant.WebApi.Application.MenuItems.Filters;

public record GetAllMenuItemsFilter(
    string? SearchNamePhrase,
    string? Category,
    decimal? PriceFrom,
    decimal? PriceTo
);