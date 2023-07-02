using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Extensions;
using GameStore.Api.Repositories;
using GameStore.Api.Utils;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string EntityName = "Game";
    const string GetGameV1EndpointName = "GetGameV1";
    const string GetGameV2EndpointName = "GetGameV2";

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.NewVersionedApi()
                          .MapGroup("/games") // "/v{version:apiVersion}/games" - url path versioning
                          .HasApiVersion(1.0)
                          .HasApiVersion(2.0)
                          .WithParameterValidation();

        #region GET ENDPOINTS V1
        group.MapGet("/", async (IGamesRepository repo, ILoggerFactory loggerFactory) =>
            Responses.Success((await repo.GetAllAsync()).Select(game => game.AsGameDtoV1()))
        )
        .MapToApiVersion(1.0);

        group.MapGet("/{id}", async (IGamesRepository repo, int id, ILoggerFactory loggerFactory) =>
        {
            Game? game = await repo.GetByIdAsync(id);
            return game is not null ? Responses.Success(game.AsGameDtoV1()) : Responses.NotFound(EntityName, id);
        })
        .WithName(GetGameV1EndpointName)
        .RequireAuthorization(Policies.ReadAccess)
        .MapToApiVersion(1.0);
        #endregion

        #region GET ENDPOINTS V2
        group.MapGet("/", async (IGamesRepository repo, ILoggerFactory loggerFactory) =>
            Responses.Success((await repo.GetAllAsync()).Select(game => game.AsGameDtoV2()))
        )
        .MapToApiVersion(2.0);

        group.MapGet("/{id}", async (IGamesRepository repo, int id, ILoggerFactory loggerFactory) =>
        {
            Game? game = await repo.GetByIdAsync(id);
            return game is not null ? Responses.Success(game.AsGameDtoV2()) : Responses.NotFound(EntityName, id);
        })
        .WithName(GetGameV2EndpointName)
        .RequireAuthorization(Policies.ReadAccess)
        .MapToApiVersion(2.0);
        #endregion

        group.MapPost("/", async (IGamesRepository repo, CreateGameDto gameDto, ILoggerFactory loggerFactory) =>
        {
            Game game = gameDto.AsGameEntity();
            await repo.CreateAsync(game);
            return Responses.Created(GetGameV1EndpointName, game);
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0);

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
            return Responses.Success(existingGame.AsGameDtoV1());
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0);

        group.MapDelete("/{id}", async (IGamesRepository repo, int id, ILoggerFactory loggerFactory) =>
        {
            Game? game = await repo.GetByIdAsync(id);

            if (game is null) return Responses.NotFound(EntityName, id);

            await repo.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0);

        return group;
    }
}