using GameStore.Api.Authorization;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Extensions;
using GameStore.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepository(builder.Configuration);
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddGameStoreAuthorization();
builder.Services.AddApiVersioning(options => options.AssumeDefaultVersionWhenUnspecified = true);

var app = builder.Build();
app.UseExceptionHandler(configure => configure.ErrorHandler());
app.UseMiddleware<RequestTimingMiddleware>();
await app.Services.InitializeDbAsync();
app.UseHttpLogging();
app.MapGamesEndpoints();
app.Run();
