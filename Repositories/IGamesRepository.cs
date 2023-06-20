using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public interface IGamesRepository
{
    void Create(Game game);
    void Delete(int id);
    IEnumerable<Game> GetAll();
    Game? GetById(int id);
    void Update(Game updatedGame);
}
