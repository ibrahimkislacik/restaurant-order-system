using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using CFusionRestaurant.ViewModel.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CFusionRestaurant.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{

    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Retrieves a list of categories.
    /// </summary>
    /// <returns>A list of categories.</returns>
    /// <response code="200">Returns the list of categories.</response>
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryViewModel>))]
    public async Task<IActionResult> List()
    {
        var result = await _categoryService.ListAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to retrieve.</param>
    /// <returns>The category with the specified ID.</returns>
    /// <response code="200">Returns the category.</response>
    /// <response code="404">If a category with the specified ID is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var category = await _categoryService.GetAsync(id).ConfigureAwait(false);
        if (category == null)
        {
            return NotFound($"Category with Id = {id} not found");
        }
        return Ok(category);
    }

    /// <summary>
    /// Inserts a new category.
    /// </summary>
    /// <param name="categoryInsertViewModel">The data for the new category.</param>
    /// <returns>Returns the created category's location.</returns>
    /// <response code="201">Returns the created category's location.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="401">If the request is not authenticated.</response>
    /// <response code="403">If the request is authenticated but does not have the required role.</response>
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles =UserRoles.Admin)]
    public async Task<IActionResult> Insert([FromBody] CategoryInsertRequestViewModel categoryInsertViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _categoryService.InsertAsync(categoryInsertViewModel);

        return Created($"/category/{result}", result);
    }

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>Returns an empty response if the category is deleted successfully.</returns>
    /// <response code="204">Returns if the category is deleted successfully.</response>
    /// <response code="401">If the request is not authenticated.</response>
    /// <response code="403">If the request is authenticated but does not have the required role.</response>
    /// <response code="404">If the category to delete is not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="categoryUpdateViewModel">The updated data for the category.</param>
    /// <returns>Returns an empty response if the category is updated successfully.</returns>
    /// <response code="204">Returns if the category is updated successfully.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="401">If the request is not authenticated.</response>
    /// <response code="403">If the request is authenticated but does not have the required role.</response>
    /// <response code="404">If the category to update is not found.</response>
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update([FromBody] CategoryUpdateRequestViewModel categoryUpdateViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _categoryService.UpdateAsync(categoryUpdateViewModel);
        return NoContent();
    }

}
