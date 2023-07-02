namespace GameStore.Api.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddGameStoreCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>()
                    ?? throw new InvalidOperationException("AllowedOrigins is not set");
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .WithExposedHeaders("X-Pagination");
            });
        });

        return services;
    }
}