using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Extensions;

public static class GameExtensions
{
    public static GameDtoV1 AsGameDtoV1(this Game game)
    {
        return new GameDtoV1(
            game.Id,
            game.Name,
            game.Genre,
            game.Price,
            game.ReleaseDate,
            game.ImageUrl
        );
    }

    public static GameDtoV2 AsGameDtoV2(this Game game)
    {
        return new GameDtoV2(
            game.Id,
            game.Name,
            game.Genre,
            game.Price,
            game.Price - (game.Price * 0.3m),
            game.ReleaseDate,
            game.ImageUrl
        );
    }

    public static Game AsGameEntity(this CreateGameDto gameDto)
    {
        return new()
        {
            Name = gameDto.Name,
            Genre = gameDto.Genre,
            Price = gameDto.Price,
            ReleaseDate = gameDto.ReleaseDate,
            ImageUrl = gameDto.ImageUrl
        };
    }
}