namespace online_fashion_shopping_api.Models
{
    public class UserType
    {
        public const string Client = "client";
        public const string Stylist = "stylist";
    }
    public class User
    {
        public required string Email { get; set; }
        public required string DisplayName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string UserType { get; set; }
        public string Bio { get; set; } = "";
        public string[] Styles { get; set; } = [];
    }

    public class CreateUser: User {
        public required string Password { get; set; }
        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "email", Email },
                { "display_name", DisplayName },
                { "phone_number", PhoneNumber },
                { "user_type", UserType },
                { "bio", Bio  },
                { "styles", Styles  },
                { "password", Password }
            };
        }           
    }
}