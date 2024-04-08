using Google.Cloud.Firestore;
using online_fashion_shopping_api.Models;

namespace online_fashion_shopping_api.Responses
{
    public class UserResponse: User {
        public required string Id { get; set; }
        public virtual Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "id", Id },
                { "email", Email },
                { "display_name", DisplayName },
                { "phone_number", PhoneNumber },
                { "user_type", UserType },
                { "bio", Bio },
                { "styles", Styles }
            };
        }

        public static UserResponse FromFirestore(DocumentSnapshot snapshot)
        {
            return new UserResponse
            {
                Id = snapshot.Id,
                Email = snapshot.GetValue<string>("email") ?? string.Empty,
                DisplayName = snapshot.GetValue<string>("display_name") ?? string.Empty,
                PhoneNumber = snapshot.GetValue<string>("phone_number") ?? string.Empty,
                UserType = snapshot.GetValue<string>("user_type") ?? string.Empty,
                Bio = snapshot.GetValue<string>("bio") ?? string.Empty,
                Styles = snapshot.GetValue<string[]>("styles") ?? []
            };
        }
    }
}