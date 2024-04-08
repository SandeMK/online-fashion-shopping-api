using Google.Cloud.Firestore;
namespace online_fashion_shopping_api.Responses
{
    public class UserLoginResponse: UserResponse
    {
        public required string CustomToken { get; set; }
        public override Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "id", Id },
                { "email", Email },
                { "display_name", DisplayName },
                { "phone_number", PhoneNumber },
                { "user_type", UserType },
                { "bio", Bio },
                { "styles", Styles },
                { "custom_token", CustomToken }
            };
        }
        public static UserLoginResponse FromFirestore(DocumentSnapshot snapshot, string customToken)
        {
            return new UserLoginResponse
            {
                Id = snapshot.Id,
                Email = snapshot.GetValue<string>("email") ?? string.Empty,
                DisplayName = snapshot.GetValue<string>("display_name") ?? string.Empty,
                PhoneNumber = snapshot.GetValue<string>("phone_number") ?? string.Empty,
                UserType = snapshot.GetValue<string>("user_type") ?? string.Empty,
                Bio = snapshot.GetValue<string>("bio") ?? string.Empty,
                Styles = snapshot.GetValue<string[]>("styles") ?? [],
                CustomToken = customToken ?? string.Empty
            };
        }
    }
}