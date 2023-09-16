using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TastyRestaurant.WebApi.Application.Orders.Commands;
using TastyRestaurant.WebApi.Application.Orders.Exceptions;
using TastyRestaurant.WebApi.Application.Orders.Models;
using TastyRestaurant.WebApi.Application.Orders.Queries;
using TastyRestaurant.WebApi.Contracts.V1;
using TastyRestaurant.WebApi.Contracts.V1.Requests;
using TastyRestaurant.WebApi.Helpers;
using TastyRestaurant.WebApi.Mappers;

namespace TastyRestaurant.WebApi.Controllers.V1;

[Authorize]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ISender _mediator;

    public OrdersController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route(ApiRoutes.Orders.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOrdersFilterRequest queryFilterRequest, [FromQuery] PaginationFilterRequest paginationQuery)
    {
        var filter = queryFilterRequest.MapToGetAllOrdersFilter();
        var pagination = paginationQuery.MapToPaginationFilter();
        var query = new GetAllOrdersQuery(filter, pagination);
        var orders = await _mediator.Send(query);
        var ordersResponse = orders.MapToOrderResponseList();

        return Ok(ordersResponse);
    }

    [HttpGet]
    [Route(ApiRoutes.Orders.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid orderId)
    {
        try
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(orderId));
            var orderResponse = order.MapToOrderResponse();

            return Ok(orderResponse);
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [Route(ApiRoutes.Orders.Create)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest createOrderRequest)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var userGuid = userId is null ? Guid.Empty : Guid.Parse(userId);
        var createOrderCommand = new CreateOrderCommand(userGuid, createOrderRequest.OrderItems.Select(x => new OrderItemModel(x.MenuItemId, x.Quantity)));
        var createdOrder = await _mediator.Send(createOrderCommand);

        var locationUrl = UrlHelper.GetResourceLocationUrl(HttpContext, ApiRoutes.Orders.Get.Replace("{orderId:guid}", createdOrder.Id.ToString()));

        return Created(locationUrl, null);
    }

    [HttpPut]
    [Route(ApiRoutes.Orders.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid orderId, [FromBody] UpdateOrderRequest updateOrderRequest)
    {
        try
        {
            var updateOrderCommand = new UpdateOrderCommand(
                orderId,
                updateOrderRequest.Status.MapToOrderStatusEnum(),
                updateOrderRequest.OrderItems.Select(x => new OrderItemModel(x.MenuItemId, x.Quantity)));

            var updatedOrder = await _mediator.Send(updateOrderCommand);

            return Ok(updatedOrder);
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}