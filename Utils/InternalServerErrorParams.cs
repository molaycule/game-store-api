namespace GameStore.Api.Utils;

public record InternalServerErrorParams(
	IHostEnvironment? Environment,
	ILogger? Logger,
	string? Message,
	Exception? Exception
);
