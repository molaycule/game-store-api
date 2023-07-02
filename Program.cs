using GameStore.Api.Endpoints;
using GameStore.Api.Extensions;
using GameStore.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepository(builder.Configuration);
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddGameStoreAuthorization();
builder.Services.AddApiVersioning(options => options.AssumeDefaultVersionWhenUnspecified = true);
builder.Services.AddGameStoreCors(builder.Configuration);

var app = builder.Build();
await app.Services.InitializeDbAsync();
app.UseExceptionHandler(configure => configure.ErrorHandler());
app.UseMiddleware<RequestTimingMiddleware>();
app.UseHttpLogging();
app.UseCors();
app.MapGamesEndpoints();
app.Run();
