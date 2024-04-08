using System.Text.Json.Serialization;

namespace online_fashion_shopping_api.Requests
{
    public class UserUpdateRequest
    {
        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; }
        
        public string? Bio { get; set; }
        public string[]? Styles { get; set; }
    }
}