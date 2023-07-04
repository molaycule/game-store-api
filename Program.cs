using GameStore.Api.Endpoints;
using GameStore.Api.Extensions;
using GameStore.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddRepository(config);
builder.Services.AddAuthentication().AddJwtBearer().AddJwtBearer("Auth0");
builder.Services.AddGameStoreAuthorization();
builder.Services.AddGameStoreCors(config);
builder.Services.AddGameStoreApiVersioning();
builder.Services.AddGameStoreSwagger();
builder.Services.AddCloudinaryService(config);

var app = builder.Build();
await app.Services.InitializeDbAsync();
app.UseExceptionHandler(configure => configure.ErrorHandler());
app.UseMiddleware<RequestTimingMiddleware>();
app.UseHttpLogging();
app.UseCors();
app.MapGamesEndpoints();
app.MapImagesEndpoints();
app.UseGameStoreSwagger();
app.Run();
