namespace online_fashion_shopping_api.Requests
{
    public class UserRegistrationRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string DisplayName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string UserType { get; set; }
    }
}