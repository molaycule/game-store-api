using GameStore.Api.Authorization;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Extensions;
using GameStore.Api.Repositories;
using GameStore.Api.Utils;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string EntityName = "Game";
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/games").WithParameterValidation();

        group.MapGet("/", async (IGamesRepository repo, ILoggerFactory loggerFactory) =>
            Responses.Success((await repo.GetAllAsync()).Select(game => game.AsGameDto()))
        );

        group.MapGet("/{id}", async (IGamesRepository repo, int id, ILoggerFactory loggerFactory) =>
        {
            Game? game = await repo.GetByIdAsync(id);
            return game is not null ? Responses.Success(game) : Responses.NotFound(EntityName, id);
        })
        .WithName(GetGameEndpointName)
        .RequireAuthorization(Policies.ReadAccess);

        group.MapPost("/", async (IGamesRepository repo, CreateGameDto gameDto, ILoggerFactory loggerFactory) =>
        {
            Game game = gameDto.AsGameEntity();
            await repo.CreateAsync(game);
            return Responses.Created(GetGameEndpointName, game);
        })
        .RequireAuthorization(Policies.WriteAccess);

        group.MapPut("/{id}", async (IGamesRepository repo, int id, UpdateGameDto updatedGameDto, ILoggerFactory loggerFactory) =>
        {
            Game? existingGame = await repo.GetByIdAsync(id);

            if (existingGame is null) return Responses.NotFound(EntityName, id);

            existingGame.Name = updatedGameDto.Name;
            existingGame.Genre = updatedGameDto.Genre;
            existingGame.Price = updatedGameDto.Price;
            existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
            existingGame.ImageUrl = updatedGameDto.ImageUrl;

            await repo.UpdateAsync(existingGame);
            return Responses.Success(existingGame);
        })
        .RequireAuthorization(Policies.WriteAccess);

        group.MapDelete("/{id}", async (IGamesRepository repo, int id, ILoggerFactory loggerFactory) =>
        {
            Game? game = await repo.GetByIdAsync(id);

            if (game is null) return Responses.NotFound(EntityName, id);

            await repo.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess);

        return group;
    }
}