using GameStore.Api.Data;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;

public class EFGameRepository : IGamesRepository
{
    private readonly GameStoreContext dbContext;
    private readonly ILogger<EFGameRepository> logger;

    public EFGameRepository(GameStoreContext dbContext, ILogger<EFGameRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<IEnumerable<Game>> GetAllAsync() => await dbContext.Games.AsNoTracking().ToListAsync();

    public async Task<Game?> GetByIdAsync(int id) => await dbContext.Games.FindAsync(id);

    public async Task CreateAsync(Game game)
    {
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Created game {Name} with price {Price}.", game.Name, game.Price);
    }

    public async Task UpdateAsync(Game updatedGame)
    {
        dbContext.Games.Update(updatedGame);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
    }
}