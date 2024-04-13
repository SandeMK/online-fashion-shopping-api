using System.Text.Json.Serialization;

namespace online_fashion_shopping_api.Models
{
    public class ChatInitiateRequest
    {
        [JsonPropertyName("sender_id")]
        public required string SenderId { get; set; }
        [JsonPropertyName("receiver_id")]
        public required string ReceiverId { get; set; }
    }
}