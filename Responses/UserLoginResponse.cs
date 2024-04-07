using online_fashion_shopping_api.Models;
namespace online_fashion_shopping_api.Responses
{
    public class UserLoginResponse: User
    {
        public required string CustomToken { get; set; }
    }
}