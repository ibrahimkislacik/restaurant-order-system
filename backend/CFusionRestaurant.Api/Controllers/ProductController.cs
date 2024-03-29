using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using CFusionRestaurant.ViewModel.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CFusionRestaurant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves a list of products based on the provided day of the week.
        /// </summary>
        /// <param name="day">The day of the week. Ex: Sunday=0, Monday=1 and etc.</param>
        /// <returns>A list of products.</returns>
        /// <response code="200">Returns the list of products.</response>
        [HttpGet("list/{day}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductViewModel>))]
        public async Task<IActionResult> List(DayOfWeek day)
        {
            var result = await _productService.ListAsync(day);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID.</returns>
        /// <response code="200">Returns the product.</response>
        /// <response code="404">If a product with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productService.GetAsync(id).ConfigureAwait(false);
            if (product == null)
            {
                return NotFound($"Product with Id = {id} not found");
            }
            return Ok(product);
        }

        /// <summary>
        /// Inserts a new product.
        /// </summary>
        /// <param name="productInsertViewModel">The data for the new product.</param>
        /// <returns>Returns the created product's location.</returns>
        /// <response code="201">Returns the created product's location.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the request is authenticated but does not have the required role.</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Insert([FromBody] ProductInsertRequestViewModel productInsertViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.InsertAsync(productInsertViewModel);

            return Created($"/product/{result}", result);
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>Returns an empty response if the product is deleted successfully.</returns>
        /// <response code="200">Returns an empty response if the product is deleted successfully.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the request is authenticated but does not have the required role.</response>
        /// <response code="404">If the product to delete is not found.</response>

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="productUpdateViewModel">The updated data for the product.</param>
        /// <returns>Returns an empty response if the product is updated successfully.</returns>
        /// <response code="200">Returns an empty response if the product is updated successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the request is authenticated but does not have the required role.</response>
        /// <response code="404">If the product to update is not found.</response>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Update([FromBody] ProductUpdateRequestViewModel productUpdateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _productService.UpdateAsync(productUpdateViewModel);
            return Ok();
        }
    }
}
