using System.Text.Json.Serialization;

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
    }
}