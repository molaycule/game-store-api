using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepository(builder.Configuration);

var app = builder.Build();
app.Services.InitializeDb();
app.MapGamesEndpoints();
app.Run();
