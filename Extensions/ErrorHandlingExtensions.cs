using GameStore.Api.Utils;
using Microsoft.AspNetCore.Diagnostics;

namespace GameStore.Api.Extensions;

public static class ErrorHandlingExtensions
{
	public static void ErrorHandler(this IApplicationBuilder app) =>
		app.Run(async context =>
		{
			var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
			var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("ErrorHandling");
			var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
			var exception = exceptionHandlerPathFeature?.Error;
			var path = exceptionHandlerPathFeature?.Path;
			var endpoint = exceptionHandlerPathFeature?.Endpoint;
			var message = ErrorMessages.ConfigureMessage(endpoint?.DisplayName, path?.Split('/').Last());
			await Responses.InternalServerError(new(environment, logger, message, exception)).ExecuteAsync(context);
		});
}
