namespace GameStore.Api.Interfaces;

public interface IGameDtoV1
{
	int Id { get; set; }
	string Name { get; set; }
	string Genre { get; set; }
	decimal Price { get; set; }
	DateTime ReleaseDate { get; set; }
	string ImageUrl { get; set; }
}
