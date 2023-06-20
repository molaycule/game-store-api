using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Extensions;

public static class GameExtensions
{
    public static GameDto AsGameDto(this Game game)
    {
        return new GameDto(
            game.Id,
            game.Name,
            game.Genre,
            game.Price,
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