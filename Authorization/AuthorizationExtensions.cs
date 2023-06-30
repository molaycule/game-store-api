namespace GameStore.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddGameStoreAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.ReadAccess, policy => policy.RequireClaim("scope", "games:read"));
            options.AddPolicy(Policies.WriteAccess, policy => policy.RequireClaim("scope", "games:write").RequireRole("Admin"));
        });

        return services;
    }
}