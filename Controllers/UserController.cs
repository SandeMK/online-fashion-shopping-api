using Microsoft.AspNetCore.Mvc;
using online_fashion_shopping_api.Requests;
using online_fashion_shopping_api.Responses;
using online_fashion_shopping_api.Services;

namespace online_fashion_shopping_api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest user)
        {
           try
            {
                var newUser = await _userService.Register(user);
                return Ok(newUser.ToDictionary());
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            try
            {
                var loggedInUser = await _userService.Login(user);
                return Ok(loggedInUser.ToDictionary());
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

        [HttpPost]
        [Route("{id}/update")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateRequest user)
        {
            try
            {
                var updatedUser = await _userService.UpdateProfile(id, user);
                return Ok(updatedUser.ToDictionary());
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

    }
}