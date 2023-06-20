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

        group.MapGet("/", (IGamesRepository repo) => SuccessResponse(repo.GetAll().Select(game => game.AsGameDto())));

        group.MapGet("/{id}", (IGamesRepository repo, int id) =>
        {
            Game? game = repo.GetById(id);
            return game is not null ? SuccessResponse(game) : NotFoundResponse(id);
        }).WithName(GetGameEndpointName);

        group.MapPost("/", (IGamesRepository repo, CreateGameDto gameDto) =>
        {
            Game game = gameDto.AsGameEntity();
            repo.Create(game);
            return CreatedResponse(game);
        });

        group.MapPut("/{id}", (IGamesRepository repo, int id, UpdateGameDto updatedGameDto) =>
        {
            Game? existingGame = repo.GetById(id);

            if (existingGame is null) return NotFoundResponse(id);

            existingGame.Name = updatedGameDto.Name;
            existingGame.Genre = updatedGameDto.Genre;
            existingGame.Price = updatedGameDto.Price;
            existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
            existingGame.ImageUrl = updatedGameDto.ImageUrl;

            repo.Update(existingGame);

            return SuccessResponse(existingGame);
        });

        group.MapDelete("/{id}", (IGamesRepository repo, int id) =>
        {
            Game? game = repo.GetById(id);

            if (game is null) return NotFoundResponse(id);

            repo.Delete(id);

            return Results.NoContent();
        });

        return group;
    }
}