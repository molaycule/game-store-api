using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepository(builder.Configuration);

var app = builder.Build();
await app.Services.InitializeDbAsync();
app.MapGamesEndpoints();
app.Run();
