using CFusionRestaurant.Api.Infrastructure;
using CFusionRestaurant.BusinessLayer.Abstract.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement;
using CFusionRestaurant.ViewModel.OrderManagement.Request;
using CFusionRestaurant.ViewModel.OrderManagement.Response;
using CFusionRestaurant.ViewModel.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CFusionRestaurant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(RequestResponseLoggingFilter))] // Apply the filter to this controller
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        /// <summary>
        /// Retrieves a list of orders.
        /// </summary>
        /// <returns>A list of orders.</returns>
        /// <response code="200">Returns the list of orders.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the request is authenticated but does not have the required role.</response>
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderViewModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> List()
        {
            var result = await _orderService.ListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order with the specified ID.</returns>
        /// <response code="200">Returns the order.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="404">If a order with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _orderService.GetAsync(id).ConfigureAwait(false);
            if (order == null)
            {
                return NotFound($"Order with Id = {id} not found");
            }
            return Ok(order);
        }

        /// <summary>
        /// Retrieves a list of user's orders.
        /// </summary>
        /// <returns>A list of user's orders.</returns>
        /// <response code="200">Returns the list of orders.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the request is authenticated but does not have the required role.</response>
        [HttpGet("my-orders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderViewModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> MyOrders()
        {
            var result = await _orderService.ListForUserAsync();
            return Ok(result);
        }


        /// <summary>
        /// Inserts a new order.
        /// </summary>
        /// <param name="orderInsertViewModel">The data for the new order.</param>
        /// <returns>Returns the created order no.</returns>
        /// <response code="200">Returns the created order no.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">If the request is not authenticated.</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderInsertResponseViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> Insert([FromBody] OrderInsertRequestViewModel orderInsertViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _orderService.InsertAsync(orderInsertViewModel);
            return Ok(result);
        }

    }
}
