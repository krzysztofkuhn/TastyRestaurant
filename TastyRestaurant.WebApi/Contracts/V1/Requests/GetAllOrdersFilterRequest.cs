using Microsoft.AspNetCore.Mvc;
using TastyRestaurant.WebApi.Contracts.V1.Consts;
namespace TastyRestaurant.WebApi.Contracts.V1.Requests;

public record GetAllOrdersFilterRequest
{
    [FromQuery(Name="status")]
    public OrderStatusEnumContract? Status { get; init; }
    
    [FromQuery(Name = "priceFrom")]
    public decimal? PriceFrom { get; init; }
    
    [FromQuery(Name = "priceTo")]
    public decimal? PriceTo { get; init; }
    
    [FromQuery(Name = "userId")]
    public Guid? UserId { get; init; }
    
    [FromQuery(Name = "creationDateFrom")]
    public DateTime? CreationDateFrom { get; init; }
    
    [FromQuery(Name = "creationDateTo")]
    public DateTime? CreationDateTo { get; init; }
    
    [FromQuery(Name = "updateDateFrom")]
    public DateTime? UpdateDateFrom { get; init; }
    
    [FromQuery(Name = "updateDateTo")]
    public DateTime? UpdateDateTo { get; init; }
}