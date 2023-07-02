using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record GetGamesDto(
    int PageNumber = 1,
    int PageSize = 5
);

public record GameDtoV1(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string ImageUrl
);

public record GameDtoV2(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    decimal DiscountedPrice,
    DateTime ReleaseDate,
    string ImageUrl
);

public record CreateGameDto(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(20)] string Genre,
    [Range(1, 100)] decimal Price,
    DateTime ReleaseDate,
    [Url][StringLength(100)] string ImageUrl
);

public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(20)] string Genre,
    [Range(1, 100)] decimal Price,
    DateTime ReleaseDate,
    [Url][StringLength(100)] string ImageUrl
);
