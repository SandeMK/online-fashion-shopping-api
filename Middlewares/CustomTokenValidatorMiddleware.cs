using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;

namespace online_fashion_shopping_api.Middlewares
{
    public class CustomTokenValidatorMiddleware(RequestDelegate next, FirebaseAuth firebaseAuth)
    {
    private readonly RequestDelegate _next = next;
    private readonly FirebaseAuth _firebaseAuth = firebaseAuth;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.Contains("/api/user/register") ||
                context.Request.Path.Value.Contains("/api/user/login"))
            {
                await _next(context);
                return;
            }

            string? authorizationHeader = context.Request.Headers["Authorization"];
            if (authorizationHeader == null || !authorizationHeader.ToString().StartsWith("Bearer "))
            {
                context.Response.StatusCode = 401;
                return;
            }

            var token = authorizationHeader.ToString().Replace("Bearer ", string.Empty);
            if (token != null)
            {
                try
                {
                    string key = Environment.GetEnvironmentVariable("SECRET_KEY");
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedSecurityToken);
                    context.Items["UserId"] = securityToken;
                }
                catch (SecurityTokenValidationException e)
                {
                    context.Response.StatusCode = 401;
                    var respone = new
                    {
                        Message = "Invalid token",
                        Error = e.Message
                    };
                    await context.Response.WriteAsJsonAsync(respone);
                    return;
                }
            }
            await _next(context);
        }
    }
}