namespace online_fashion_shopping_api.Models
{
    public class UserType
    {
        public const string Client = "Client";
        public const string Stylist = "stylist";
    }
    public class User
    {
        public string? Id { get; set; }
        public required string Email { get; set; }
        public required string DisplayName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string UserType { get; set; }
    }

    
}