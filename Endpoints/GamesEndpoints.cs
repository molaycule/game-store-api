using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Extensions;
using GameStore.Api.Repositories;
using GameStore.Api.Utils;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
	private const string EntityName = "Game";
	private const string GetGameV1EndpointName = "GetGameV1";
	private const string GetGameV2EndpointName = "GetGameV2";

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
		group.MapGet("/", async Task<Results<Ok<SuccessWithFieldsResponse<IEnumerable<Dictionary<string, object?>>>>, Ok<SuccessWithDataResponse<IEnumerable<GameDtoV1>>>>> (
			IGamesRepository repo,
			ILoggerFactory loggerFactory,
			[AsParameters] GetGamesDto request,
			[AsParameters] QueryFieldsDto query,
			HttpContext context) =>
		{
			await AddPaginationToResponseHeader(repo, request, context);
			var results = (await repo.GetAllAsync(request.PageNumber, request.PageSize, request.Filter))
									 .Select(game => game.AsGameDtoV1());
			return query.Fields is not null
				? Responses.SuccessWithFields(results.FilterList(query.Fields))
				: Responses.SuccessWithData(results);
		})
		.MapToApiVersion(1.0)
		.WithSummary("Get all games")
		.WithDescription("Get all available games with pagination and allow to filter them by name or genre.");

		group.MapGet("/{id}", async Task<Results<Ok<SuccessWithFieldsResponse<Dictionary<string, object?>>>, Ok<SuccessWithDataResponse<GameDtoV1>>, NotFound<FailResponse>>> (
			IGamesRepository repo, int id, ILoggerFactory loggerFactory, [AsParameters] QueryFieldsDto query) =>
		{
			var game = await repo.GetByIdAsync(id);

			if (game is null) return Responses.NotFound(EntityName, id);

			return query.Fields is not null
				? Responses.SuccessWithFields(game.AsGameDtoV1().Filter(query.Fields))
				: Responses.SuccessWithData(game.AsGameDtoV1());
		})
		.WithName(GetGameV1EndpointName)
		.RequireAuthorization(Policies.ReadAccess)
		.MapToApiVersion(1.0)
		.WithSummary("Get game")
		.WithDescription("Get game by id with all details.");
		#endregion

		#region GET ENDPOINTS V2
		group.MapGet("/", async Task<Results<Ok<SuccessWithFieldsResponse<IEnumerable<Dictionary<string, object?>>>>, Ok<SuccessWithDataResponse<IEnumerable<GameDtoV2>>>>> (
			IGamesRepository repo,
			ILoggerFactory loggerFactory,
			[AsParameters] GetGamesDto request,
			[AsParameters] QueryFieldsDto query,
			HttpContext context) =>
		{
			await AddPaginationToResponseHeader(repo, request, context);
			var results = (await repo.GetAllAsync(request.PageNumber, request.PageSize, request.Filter))
									 .Select(game => game.AsGameDtoV2());
			return query.Fields is not null
				? Responses.SuccessWithFields(results.FilterList(query.Fields))
				: Responses.SuccessWithData(results);
		})
		.MapToApiVersion(2.0)
		.WithSummary("Get all games")
		.WithDescription("Get all available games with pagination and allow to filter them by name or genre.");

		group.MapGet("/{id}", async Task<Results<Ok<SuccessWithFieldsResponse<Dictionary<string, object?>>>, Ok<SuccessWithDataResponse<GameDtoV2>>, NotFound<FailResponse>>> (
			IGamesRepository repo, int id, ILoggerFactory loggerFactory, [AsParameters] QueryFieldsDto query) =>
		{
			var game = await repo.GetByIdAsync(id);

			if (game is null) return Responses.NotFound(EntityName, id);

			return query.Fields is not null
				? Responses.SuccessWithFields(game.AsGameDtoV2().Filter(query.Fields))
				: Responses.SuccessWithData(game.AsGameDtoV2());
		})
		.WithName(GetGameV2EndpointName)
		.RequireAuthorization(Policies.ReadAccess)
		.MapToApiVersion(2.0)
		.WithSummary("Get game")
		.WithDescription("Get game by id with all details.");
		#endregion

		group.MapPost("/", async Task<CreatedAtRoute<SuccessWithDataResponse<GameDtoV1>>> (
			IGamesRepository repo, CreateGameDto gameDto, ILoggerFactory loggerFactory) =>
		{
			var game = gameDto.AsGameEntity();
			await repo.CreateAsync(game);
			return Responses.Created(GetGameV1EndpointName, game.Id, game.AsGameDtoV1());
		})
		.RequireAuthorization(Policies.WriteAccess)
		.MapToApiVersion(1.0)
		.WithSummary("Create new game")
		.WithDescription("Create a new game with the specified properties.");

		group.MapPut("/{id}", async Task<Results<Ok<SuccessWithDataResponse<GameDtoV1>>, NotFound<FailResponse>>> (
			IGamesRepository repo, int id, UpdateGameDto updatedGameDto, ILoggerFactory loggerFactory) =>
		{
			var existingGame = await repo.GetByIdAsync(id);

			if (existingGame is null) return Responses.NotFound(EntityName, id);

			existingGame.Name = updatedGameDto.Name;
			existingGame.Genre = updatedGameDto.Genre;
			existingGame.Price = updatedGameDto.Price;
			existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
			existingGame.ImageUrl = updatedGameDto.ImageUrl;

			await repo.UpdateAsync(existingGame);
			return Responses.SuccessWithData(existingGame.AsGameDtoV1());
		})
		.RequireAuthorization(Policies.WriteAccess)
		.MapToApiVersion(1.0)
		.WithSummary("Update game")
		.WithDescription("Update game by id with the specified properties.");

		group.MapDelete("/{id}", async Task<Results<NotFound<FailResponse>, NoContent>> (
			IGamesRepository repo, int id, ILoggerFactory loggerFactory) =>
		{
			var game = await repo.GetByIdAsync(id);

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
