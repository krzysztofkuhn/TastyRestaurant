using MediatR;
using Microsoft.AspNetCore.Mvc;
using TastyRestaurant.WebApi.Application.Commands;
using TastyRestaurant.WebApi.Application.Exceptions;
using TastyRestaurant.WebApi.Application.Models;
using TastyRestaurant.WebApi.Application.Services;
using TastyRestaurant.WebApi.Contracts.V1;
using TastyRestaurant.WebApi.Contracts.V1.Requests;
using TastyRestaurant.WebApi.Mappers;

namespace TastyRestaurant.WebApi.Controllers.V1
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IOrdersService _ordersService;

        public OrdersController(ISender mediator, IOrdersService ordersService)
        {
            _mediator = mediator;
            _ordersService = ordersService;
        }

        [HttpGet]
        [Route(ApiRoutes.Orders.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllOrdersFilterRequest queryFilterRequest, [FromQuery] PaginationFilterRequest paginationQuery)
        {
            var filter = queryFilterRequest.MapToGetAllOrdersFilter();
            var pagination = paginationQuery.MapToPaginationFilter();
            var orders = await _ordersService.GetAllOrdersAsync(filter, pagination);
            var ordersResponse = orders.MapToOrderResponseList();

            return Ok(ordersResponse);
        }

        [HttpGet]
        [Route(ApiRoutes.Orders.Get)]
        public async Task<IActionResult> Get([FromRoute] uint orderId)
        {
            try
            {
                var order = await _ordersService.GetOrderByIdAsync(orderId);
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
            var createOrderCommand = new CreateOrderCommand(createOrderRequest.UserId, createOrderRequest.OrderItems.Select(x => new OrderItemModel(x.MenuItemId, x.Quantity)));
            var createdOrder = await _mediator.Send(createOrderCommand);

            var locationUrl = UrlHelper.GetResourceLocationUrl(HttpContext, ApiRoutes.Orders.Get.Replace("{orderId}", createdOrder.Id.ToString()));

            return Created(locationUrl, null);
        }        
        
        [HttpPut]
        [Route(ApiRoutes.Orders.Update)]
        public async Task<IActionResult> Update([FromRoute] uint orderId, [FromBody] UpdateOrderRequest updateOrderRequest)
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
}