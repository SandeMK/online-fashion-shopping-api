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
                object newUser = await _userService.Register(user);
                return Ok(newUser);
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
                object loggedInUser = await _userService.Login(user);
                return Ok(loggedInUser);
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
                object updatedUser = await _userService.UpdateProfile(id, user);
                return Ok(updatedUser);
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

    }
}