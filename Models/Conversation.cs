using System.Text.Json.Serialization;
using Google.Cloud.Firestore;

namespace online_fashion_shopping_api.Models
{
    public class Message
    {
        public required string Id { get; set; }
        [JsonPropertyName("sender_id")]
        public required string SenderId { get; set; }
        public required string Content { get; set; }
        public required DateTime Timestamp { get; set; }

         public static Message FromFirestore(DocumentSnapshot doc)
         {
             return new Message
             {
                 Id = doc.Id,
                 SenderId = doc.GetValue<string>("sender_id"),
                 Content = doc.GetValue<string>("content"),
                 Timestamp = doc.GetValue<DateTime>("timestamp")
             };
         
         }
    }

    public class Conversation
    {
        public required string Id { get; set; }
        public required string[] Members { get; set; }
        public required Message[] Messages { get; set; }

        public static Conversation FromFirestore(DocumentSnapshot doc)
        {
            return new Conversation
            {
                Id = doc.Id,
                Members = doc.GetValue<string[]>("members"),
                Messages = doc.GetValue<Message[]>("messages")
            };
        }

         public Dictionary<string, object> ToDictionary()
         {
             return new Dictionary<string, object>
             {
                 { "id", Id },
                 { "members", Members },
                 { "messages", Messages }
             };
         }

    }
}