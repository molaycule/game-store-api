using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GameStore.Api.Utils;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
	private readonly IApiVersionDescriptionProvider provider;

	public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

	public void Configure(SwaggerGenOptions options)
	{
		foreach (var description in provider.ApiVersionDescriptions)
		{
			options.SwaggerDoc(
				description.GroupName,
				new()
				{
					Title = $"Game Store API {description.ApiVersion}",
					Version = description.ApiVersion.ToString(),
					Description = "Manages the games catalog.",
				});
		}
	}
}
