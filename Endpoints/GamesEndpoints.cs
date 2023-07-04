using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Extensions;
using GameStore.Api.Repositories;
using GameStore.Api.Utils;
using Microsoft.AspNetCore.Http.HttpResults;

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
                          .WithParameterValidation()
                          .WithOpenApi()
                          .WithTags("Games");

        #region GET ENDPOINTS V1
        group.MapGet("/", async (
            IGamesRepository repo,
            ILoggerFactory loggerFactory,
            [AsParameters] GetGamesDto request,
            HttpContext context) =>
        {
            await AddPaginationToResponseHeader(repo, request, context);
            return Responses.Success((await repo.GetAllAsync(request.PageNumber, request.PageSize, request.Filter))
                                                .Select(game => game.AsGameDtoV1()));
        })
        .MapToApiVersion(1.0)
        .WithSummary("Get all games")
        .WithDescription("Get all available games with pagination and allow to filter them by name or genre.");

        group.MapGet("/{id}", async Task<Results<Ok<SuccessResponse<GameDtoV1>>, NotFound<FailResponse>>> (
            IGamesRepository repo, int id, ILoggerFactory loggerFactory) =>
        {
            Game? game = await repo.GetByIdAsync(id);
            return game is not null ? Responses.Success(game.AsGameDtoV1()) : Responses.NotFound(EntityName, id);
        })
        .WithName(GetGameV1EndpointName)
        .RequireAuthorization(Policies.ReadAccess)
        .MapToApiVersion(1.0)
        .WithSummary("Get game")
        .WithDescription("Get game by id with all details.");
        #endregion

        #region GET ENDPOINTS V2
        group.MapGet("/", async (
            IGamesRepository repo,
            ILoggerFactory loggerFactory,
            [AsParameters] GetGamesDto request,
            HttpContext context) =>
        {
            await AddPaginationToResponseHeader(repo, request, context);
            return Responses.Success((await repo.GetAllAsync(request.PageNumber, request.PageSize, request.Filter))
                                                .Select(game => game.AsGameDtoV2()));
        })
        .MapToApiVersion(2.0)
        .WithSummary("Get all games")
        .WithDescription("Get all available games with pagination and allow to filter them by name or genre.");

        group.MapGet("/{id}", async Task<Results<Ok<SuccessResponse<GameDtoV2>>, NotFound<FailResponse>>> (
            IGamesRepository repo, int id, ILoggerFactory loggerFactory) =>
        {
            Game? game = await repo.GetByIdAsync(id);
            return game is not null ? Responses.Success(game.AsGameDtoV2()) : Responses.NotFound(EntityName, id);
        })
        .WithName(GetGameV2EndpointName)
        .RequireAuthorization(Policies.ReadAccess)
        .MapToApiVersion(2.0)
        .WithSummary("Get game")
        .WithDescription("Get game by id with all details.");
        #endregion

        group.MapPost("/", async Task<CreatedAtRoute<SuccessResponse<GameDtoV1>>> (
            IGamesRepository repo, CreateGameDto gameDto, ILoggerFactory loggerFactory) =>
        {
            Game game = gameDto.AsGameEntity();
            await repo.CreateAsync(game);
            return Responses.Created(GetGameV1EndpointName, game.Id, game.AsGameDtoV1());
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0)
        .WithSummary("Create new game")
        .WithDescription("Create a new game with the specified properties.");

        group.MapPut("/{id}", async Task<Results<Ok<SuccessResponse<GameDtoV1>>, NotFound<FailResponse>>> (
            IGamesRepository repo, int id, UpdateGameDto updatedGameDto, ILoggerFactory loggerFactory) =>
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
        .MapToApiVersion(1.0)
        .WithSummary("Update game")
        .WithDescription("Update game by id with the specified properties.");

        group.MapDelete("/{id}", async Task<Results<NotFound<FailResponse>, NoContent>> (
            IGamesRepository repo, int id, ILoggerFactory loggerFactory) =>
        {
            Game? game = await repo.GetByIdAsync(id);

            if (game is null) return Responses.NotFound(EntityName, id);

            await repo.DeleteAsync(id);
            return TypedResults.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0)
        .WithSummary("Delete game")
        .WithDescription("Delete game with the specified id.");

        return group;
    }

    private static async Task AddPaginationToResponseHeader(IGamesRepository repo, GetGamesDto request, HttpContext context)
    {
        var totalCount = await repo.CountAsync(request.Filter);
        context.Response.AddPaginationHeader(request.PageNumber, request.PageSize, totalCount);
    }
}