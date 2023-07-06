using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Extensions;

public static class GameExtensions
{
	public static GameDtoV1 AsGameDtoV1(this Game game) => new GameDtoV1
	{
		Id = game.Id,
		Name = game.Name,
		Genre = game.Genre,
		Price = game.Price,
		ReleaseDate = game.ReleaseDate,
		ImageUrl = game.ImageUrl
	};

	public static GameDtoV2 AsGameDtoV2(this Game game) => new GameDtoV2
	{
		Id = game.Id,
		Name = game.Name,
		Genre = game.Genre,
		Price = game.Price,
		DiscountedPrice = game.Price - (game.Price * 0.3m),
		ReleaseDate = game.ReleaseDate,
		ImageUrl = game.ImageUrl
	};

	public static Game AsGameEntity(this CreateGameDto gameDto) => new()
	{
		Name = gameDto.Name,
		Genre = gameDto.Genre,
		Price = gameDto.Price,
		ReleaseDate = gameDto.ReleaseDate,
		ImageUrl = gameDto.ImageUrl
	};
}
