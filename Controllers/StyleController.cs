using Microsoft.AspNetCore.Mvc;
using online_fashion_shopping_api.Models;
using online_fashion_shopping_api.Responses;
using online_fashion_shopping_api.Services;

namespace online_fashion_shopping_api.Controllers
{
    [ApiController]
    [Route("api/styles")]
    public class StyleController(StyleService styleService) : ControllerBase
    {
        private readonly StyleService _styleService = styleService;

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllStyles()
        {
            try
            {
                object styles = await _styleService.GetStyles();
                return Ok(styles);
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStyleById(string id)
        {
            try
            {
                Style? style = await _styleService.GetStyleById(id);
                if (style == null)
                    return NotFound(new Response { Message = "Style not found" });
                
                return Ok(style);
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }
    }

}