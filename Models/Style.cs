using System.Text.Json.Serialization;
using Google.Cloud.Firestore;

namespace online_fashion_shopping_api.Models
{
    public class Style
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        [JsonPropertyName("image_url")]
        public required string ImageUrl { get; set; }
        
        public static Style FromFirestore(DocumentSnapshot doc)
        {
            return new Style()
            {
                Id = doc.Id,
                Name = doc.GetValue<string>("name"),
                Description = doc.GetValue<string>("description"),
                ImageUrl = doc.GetValue<string>("image_url")
            };
        }
        
    }
}