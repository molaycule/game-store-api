using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class InMemGamesRepository : IGamesRepository
{
    private readonly List<Game> games = new()
    {
        new Game()
        {
            Id = 1,
            Name = "Super Mario 64",
            Genre = "Platform",
            Price = 29.99m,
            ReleaseDate = new DateTime(1996, 06, 23),
            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/6/6a/Super_Mario_64_box_cover.jpg"
        },
        new Game()
        {
            Id = 2,
            Name = "The Legend of Zelda: Ocarina of Time",
            Genre = "Adventure",
            Price = 24.99m,
            ReleaseDate = new DateTime(1998, 11, 23),
            ImageUrl = "https://placehold.co/100"
        },
        new Game()
        {
            Id = 3,
            Name = "Mario Kart 64",
            Genre = "Racing",
            Price = 19.99m,
            ReleaseDate = new DateTime(1996, 12, 14),
            ImageUrl = "https://placehold.co/100"
        }
    };

    public IEnumerable<Game> GetAll() => games;

    public Game? GetById(int id) => games.Find(game => game.Id == id);

    public void Create(Game game)
    {
        game.Id = games.Max(game => game.Id) + 1;
        games.Add(game);
    }

    public void Update(Game updatedGame)
    {
        int index = games.FindIndex(game => game.Id == updatedGame.Id);
        games[index] = updatedGame;
    }

    public void Delete(int id)
    {
        int index = games.FindIndex(game => game.Id == id);
        games.RemoveAt(index);
    }
}