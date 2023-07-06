namespace GameStore.Api.Utils;

public static class ErrorMessages
{
	private const string GamesError = "An error occurred while getting games";
	private const string GameByIdError = "An error occurred while getting game with id {Id}";
	private const string GameCreationError = "An error occurred while creating a game";
	private const string GameUpdateError = "An error occurred while updating game with id {Id}";
	private const string GameDeletionError = "An error occurred while deleting game with id {Id}";

	public static string ConfigureMessage(string? endpoint, string? id) => endpoint switch
	{
		"HTTP: GET /games/" => GamesError,
		"HTTP: POST /games/" => GameCreationError,
		"HTTP: GET /games/{id}" => GameByIdError.Replace("{Id}", id),
		"HTTP: PUT /games/{id}" => GameUpdateError.Replace("{Id}", id),
		"HTTP: DELETE /games/{id}" => GameDeletionError.Replace("{Id}", id),
		_ => "An error occurred"
	};
}
