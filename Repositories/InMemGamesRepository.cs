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

    public async Task<IEnumerable<Game>> GetAllAsync(int pageNumber, int pageSize, string? filter) =>
        await Task.FromResult(FilterGames(filter).Skip((pageNumber - 1) * pageSize).Take(pageSize));

    public async Task<Game?> GetByIdAsync(int id) => await Task.FromResult(games.Find(game => game.Id == id));

    public async Task CreateAsync(Game game)
    {
        game.Id = games.Max(game => game.Id) + 1;
        games.Add(game);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(Game updatedGame)
    {
        int index = games.FindIndex(game => game.Id == updatedGame.Id);
        games[index] = updatedGame;
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        int index = games.FindIndex(game => game.Id == id);
        games.RemoveAt(index);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(string? filter) => await Task.FromResult(FilterGames(filter).Count());

    private IEnumerable<Game> FilterGames(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter)) return games;

        return games.Where(game => game.Name.Contains(filter) || game.Genre.Contains(filter));
    }
}