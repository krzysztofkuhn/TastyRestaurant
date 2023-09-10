using Microsoft.AspNetCore.Mvc;

namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public record GetAllMenuItemsFilterRequest
{
    [FromQuery(Name = "searchNamePhrase")]
    public string? SearchNamePhrase { get; init; }

    [FromQuery(Name = "category")]
    public string? Category { get; init; }

    [FromQuery(Name = "priceFrom")]
    public decimal? PriceFrom { get; init; }

    [FromQuery(Name = "priceTo")]
    public decimal? PriceTo { get; init; }
}