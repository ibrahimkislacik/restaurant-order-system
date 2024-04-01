using CFusionRestaurant.BusinessLayer.Abstract.UserManagement;
using CFusionRestaurant.ViewModel.UserManagement.Request;
using CFusionRestaurant.ViewModel.UserManagement.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CFusionRestaurant.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticates the user using the provided credentials and generates tokens.
        /// </summary>
        /// <param name="userLoginRequestViewModel">The credentials of the user to authenticate.</param>
        /// <returns>Returns the authentication token.</returns>
        /// <response code="200">Returns the authentication token.</response>
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginResponseViewModel))]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginRequestViewModel userLoginRequestViewModel)
        {
            var result = await _userService.LoginAsync(userLoginRequestViewModel);
            return Ok(result);
        }

        /// <summary>
        /// Inserts a new user for testing API. Authentication is not required.
        /// </summary>
        /// <param name="userInsertRequestViewModel">The data for the new user.</param>
        /// <returns>Returns the created user's location.</returns>
        /// <response code="201">Returns the created user's location.</response>
        /// <response code="400">If the request data is invalid.</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Insert([FromBody] UserInsertRequestViewModel userInsertRequestViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.InsertAsync(userInsertRequestViewModel);

            return Created($"/user/{result}", result);
        }
    }
}
