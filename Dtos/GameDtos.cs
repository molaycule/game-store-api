using System.ComponentModel.DataAnnotations;
using GameStore.Api.Interfaces;

namespace GameStore.Api.Dtos;

public record QueryFieldsDto
{
	public string? Fields { get; set; }
}

public record GetGamesDto
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 5;
	public string? Filter { get; set; }
}

public record GameDtoV1 : IGameDtoV1, IDto
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string Genre { get; set; } = null!;
	public decimal Price { get; set; }
	public DateTime ReleaseDate { get; set; }
	public string ImageUrl { get; set; } = null!;
}

public record GameDtoV2 : IGameDtoV2, IDto
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string Genre { get; set; } = null!;
	public decimal Price { get; set; }
	public decimal DiscountedPrice { get; set; }
	public DateTime ReleaseDate { get; set; }
	public string ImageUrl { get; set; } = null!;
}

public record CreateGameDto
{
	[Required]
	[StringLength(50)]
	public string Name { get; init; } = null!;

	[Required]
	[StringLength(20)]
	public string Genre { get; init; } = null!;

	[Range(1, 100)]
	public decimal Price { get; init; }

	public DateTime ReleaseDate { get; init; }

	[Url]
	[StringLength(100)]
	public string ImageUrl { get; init; } = null!;
}

public record UpdateGameDto
{
	[Required]
	[StringLength(50)]
	public string Name { get; init; } = null!;

	[Required]
	[StringLength(20)]
	public string Genre { get; init; } = null!;

	[Range(1, 100)]
	public decimal Price { get; init; }

	public DateTime ReleaseDate { get; init; }

	[Url]
	[StringLength(100)]
	public string ImageUrl { get; init; } = null!;
}
