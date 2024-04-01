using BCrypt.Net;

namespace online_fashion_shopping_api.Utilities
{
    public class PasswordManager
    {
        public string HashPassword(string password)
        {
            // Generate a salt and hash the password
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Check if the provided password matches the hashed password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}