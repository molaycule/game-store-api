using GameStore.Api.Authorization;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Extensions;
using GameStore.Api.Interfaces;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static IResult SuccessResponse<T>(T data) =>
        Results.Ok(new { status = "success", data });

    private static IResult CreatedResponse<T>(T data) where T : IEntity =>
        Results.CreatedAtRoute(GetGameEndpointName, new { id = data.Id }, new
        {
            status = "success",
            data
        });

    private static IResult NotFoundResponse(int id) =>
        Results.NotFound(new { status = "fail", message = $"Game with id {id} was not found" });

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/games")
                    .WithParameterValidation();

        group.MapGet("/", async (IGamesRepository repo) =>
            SuccessResponse((await repo.GetAllAsync()).Select(game => game.AsGameDto())));

        group.MapGet("/{id}", async (IGamesRepository repo, int id) =>
        {
            Game? game = await repo.GetByIdAsync(id);
            return game is not null ? SuccessResponse(game) : NotFoundResponse(id);
        })
        .WithName(GetGameEndpointName)
        .RequireAuthorization(Policies.ReadAccess);

        group.MapPost("/", async (IGamesRepository repo, CreateGameDto gameDto) =>
        {
            Game game = gameDto.AsGameEntity();
            await repo.CreateAsync(game);
            return CreatedResponse(game);
        })
        .RequireAuthorization(Policies.WriteAccess);
        // .RequireAuthorization(policy => policy.RequireRole("Admin")); // Role based authorization

        group.MapPut("/{id}", async (IGamesRepository repo, int id, UpdateGameDto updatedGameDto) =>
        {
            Game? existingGame = await repo.GetByIdAsync(id);

            if (existingGame is null) return NotFoundResponse(id);

            existingGame.Name = updatedGameDto.Name;
            existingGame.Genre = updatedGameDto.Genre;
            existingGame.Price = updatedGameDto.Price;
            existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
            existingGame.ImageUrl = updatedGameDto.ImageUrl;

            await repo.UpdateAsync(existingGame);

            return SuccessResponse(existingGame);
        })
        .RequireAuthorization(Policies.WriteAccess);

        group.MapDelete("/{id}", async (IGamesRepository repo, int id) =>
        {
            Game? game = await repo.GetByIdAsync(id);

            if (game is null) return NotFoundResponse(id);

            await repo.DeleteAsync(id);

            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess);

        return group;
    }
}