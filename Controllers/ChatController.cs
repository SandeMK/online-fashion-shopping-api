using Microsoft.AspNetCore.Mvc;
using online_fashion_shopping_api.Models;
using online_fashion_shopping_api.Responses;
using online_fashion_shopping_api.Services;

namespace online_fashion_shopping_api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController(ChatService chatService) : ControllerBase
    {
        private readonly ChatService _chatService = chatService;

        [HttpPost]
        [Route("request")]
        public async Task<IActionResult> InitiateChat([FromBody] ChatInitiateRequest request)
        {
            try
            {
                Conversation? newChat = await _chatService.InitiateChat(request);
                if (newChat == null) return BadRequest(new Response { Message = "Failed to initiate chat" });

                return Ok(newChat.ToDictionary());
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest chat)
        {
            try
            {
                var newChat = await _chatService.SendMessage(chat);
                if (newChat == null) return BadRequest(new Response { Message = "Failed to send message" });

                return Ok(newChat.ToDictionary());
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetChatById(string id)
        {
            try
            {
                Conversation? conversation = await _chatService.GetConversation(id);
                if (conversation == null) return NotFound(new Response { Message = "Chat not found" });
                
                return Ok(conversation.ToDictionary());
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }
    }
}