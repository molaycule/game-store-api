using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public interface IGamesRepository
{
    IEnumerable<Game> GetAll();
    Game? GetById(int id);
    void Create(Game game);
    void Update(Game updatedGame);
    void Delete(int id);
}
