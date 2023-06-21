using GameStore.Api.Data;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;

public class EFGameRepository : IGamesRepository
{
    private readonly GameStoreContext dbContext;

    public EFGameRepository(GameStoreContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Game>> GetAllAsync() => await dbContext.Games.AsNoTracking().ToListAsync();

    public async Task<Game?> GetByIdAsync(int id) => await dbContext.Games.FindAsync(id);

    public async Task CreateAsync(Game game)
    {
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();
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