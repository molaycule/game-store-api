namespace GameStore.Api.Utils;

public static class ErrorMessages
{
    const string GamesError = "An error occurred while getting games";
    const string GameByIdError = "An error occurred while getting game with id {Id}";
    const string GameCreationError = "An error occurred while creating a game";
    const string GameUpdateError = "An error occurred while updating game with id {Id}";
    const string GameDeletionError = "An error occurred while deleting game with id {Id}";

    public static string ConfigureMessage(string? endpoint, string? id)
    {
        return endpoint switch
        {
            "HTTP: GET /games/" => GamesError,
            "HTTP: POST /games/" => GameCreationError,
            "HTTP: GET /games/{id}" => GameByIdError.Replace("{Id}", id),
            "HTTP: PUT /games/{id}" => GameUpdateError.Replace("{Id}", id),
            "HTTP: DELETE /games/{id}" => GameDeletionError.Replace("{Id}", id),
            _ => "An error occurred"
        };
    }
}