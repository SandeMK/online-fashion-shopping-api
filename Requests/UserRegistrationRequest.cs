using System.Text.Json.Serialization;

namespace online_fashion_shopping_api.Requests
{
    public class UserRegistrationRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        [JsonPropertyName("display_name")]
        public required string DisplayName { get; set; }

        [JsonPropertyName("phone_number")]
        public required string PhoneNumber { get; set; }

        [JsonPropertyName("user_type")]
        public required string UserType { get; set; }
    }
}