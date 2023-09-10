using MediatR;
using Microsoft.AspNetCore.Mvc;
using TastyRestaurant.WebApi.Application.MenuItems;
using TastyRestaurant.WebApi.Application.MenuItems.Queries;
using TastyRestaurant.WebApi.Contracts.V1;
using TastyRestaurant.WebApi.Contracts.V1.Requests;
using TastyRestaurant.WebApi.Mappers;

namespace TastyRestaurant.WebApi.Controllers.V1;

[ApiController]
public class MenuItemController : ControllerBase
{
    private readonly ISender _mediator;

    public MenuItemController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route(ApiRoutes.MenuItems.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllMenuItemsFilterRequest queryFilterRequest, [FromQuery] PaginationFilterRequest paginationQuery)
    {
        var filter = queryFilterRequest.MapToGetAllMenuItemsFilter();
        var pagination = paginationQuery.MapToPaginationFilter();
        var query = new GetAllMenuItemsQuery(filter, pagination);
        var menuItems = await _mediator.Send(query);
        var menuItemResponseList = menuItems.MapToMenuItemResponseList();

        return Ok(menuItemResponseList);
    }
}