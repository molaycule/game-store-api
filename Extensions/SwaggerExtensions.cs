using GameStore.Api.Utils;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GameStore.Api.Extensions;

public static class SwaggerExtensions
{
	public static IServiceCollection AddGameStoreSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen()
				.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
				.AddEndpointsApiExplorer();
		return services;
	}

	public static IApplicationBuilder UseGameStoreSwagger(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				app.DescribeApiVersions().Select(description => description.GroupName).ToList().ForEach(groupName =>
				{
					var url = $"/swagger/{groupName}/swagger.json";
					var name = groupName.ToUpperInvariant();
					options.SwaggerEndpoint(url, name);
				});
			});
		}

		return app;
	}
}
