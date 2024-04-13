using Google.Cloud.Firestore;
using online_fashion_shopping_api.Models;

namespace online_fashion_shopping_api.Responses
{
    public class GetStyleResponse
    {
        public required Style Style { get; set; }
        public required UserResponse[] Stylists { get; set; } = [];

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "style", Style.ToDictionary() },
                { "stylists", Stylists.Select(s => s.ToDictionary()).ToArray() }
            };
        }

    }
}