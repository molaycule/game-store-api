namespace GameStore.Api.Extensions;

public static class ApiVersioningExtensions
{
    public static IServiceCollection AddGameStoreApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options => options.AssumeDefaultVersionWhenUnspecified = true)
                .AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");
        return services;
    }
}