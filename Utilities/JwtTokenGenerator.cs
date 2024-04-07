using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
namespace online_fashion_shopping_api.Utilities
{
    public class JwtTokenGenerator()
    {
            public string GenerateToken(string UserId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserId),
            };

            var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
            var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: "online-fashion-shopping-api",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}