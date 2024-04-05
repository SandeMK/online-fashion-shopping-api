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
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: "online-fashion-shopping-api",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(tokenDescriptor);
        return token;
    }
}
}