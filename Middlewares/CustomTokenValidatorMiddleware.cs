using FirebaseAdmin.Auth;

namespace online_fashion_shopping_api.Middlewares
{
    public class CustomTokenValidatorMiddleware(RequestDelegate next, FirebaseAuth firebaseAuth)
    {
    private readonly RequestDelegate _next = next;
    private readonly FirebaseAuth _firebaseAuth = firebaseAuth;

        public async Task InvokeAsync(HttpContext context)
    {
        // if path is /api/user/register, skip token validation
        if (context.Request.Path.Value.Contains("/api/user/register") ||
            context.Request.Path.Value.Contains("/api/user/login"))
        {
            await _next(context);
            return;
        }
        // 1. Check for Custom Token in Authorization header
        if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            context.Response.StatusCode = 401; // Unauthorized
            return;
        }

        var token = authorizationHeader.FirstOrDefault()?.Split(' ').LastOrDefault();

            // 2. Verify Custom Token Signature (replace with your secret key)
        try
        {
            var verificationResult = await _firebaseAuth.VerifyIdTokenAsync(token);
            context.Items["UserId"] = verificationResult.Uid; // Store user ID for further use
        }
        catch (Exception)
        {
            context.Response.StatusCode = 401; // Unauthorized
            return;
        }

        await _next(context);
    }
}
}