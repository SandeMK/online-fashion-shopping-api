namespace online_fashion_shopping_api.Requests
{
    public class UserUpdateRequest
    {
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string[]? Styles { get; set; }
    }
}