using System.Text.Json.Serialization;

namespace online_fashion_shopping_api.Models
{
    public class SendMessageRequest
    {
         [JsonPropertyName("sender_id")]
        public required string SenderId { get; set; }
        public required string Content { get; set; }
        [JsonPropertyName("conversation_id")]
        public required string ConversationId { get; set; }
    }
}