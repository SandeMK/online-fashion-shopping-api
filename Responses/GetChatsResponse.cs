using System.Text.Json.Serialization;

namespace online_fashion_shopping_api.Responses
{
    public class ChatData 
    {
        [JsonPropertyName("conversation_id")]
        public required string ConversationId { get; set; }
        [JsonPropertyName("receiver_id")]
        public required string ReceiverId { get; set; }
        [JsonPropertyName("receiver_name")]
        public required string ReceiverName { get; set; }
    }
    public class GetChatsResponse
    {
        public required List<ChatData> Chats { get; set; } = [];
    }
}