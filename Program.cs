using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using online_fashion_shopping_api.Middlewares;
using online_fashion_shopping_api.Services;
using online_fashion_shopping_api.DotEnv;

var builder = WebApplication.CreateBuilder(args);

var root = Directory.GetCurrentDirectory();
var envFile = Path.Combine(root, ".env");
DotEnv.Load(envFile);

var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "service_account.json");

builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("service_account.json")
}
));

builder.Services.AddSingleton(FirebaseAuth.DefaultInstance);
builder.Services.AddSingleton(FirestoreDb.Create("online-fashion-shopping-675ab"));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<StyleService>();


builder.Services.AddFirebaseAuthentication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomTokenValidatorMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

