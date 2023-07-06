using GameStore.Api.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GameStore.Api.Extensions;

public static class AuthorizationExtensions
{
	public static IServiceCollection AddGameStoreAuthorization(this IServiceCollection services)
	{
		services.AddScoped<IClaimsTransformation, ScopeTransformation>().AddAuthorization(options =>
		{
			options.AddPolicy(Policies.ReadAccess, policy =>
				policy.RequireClaim("scope", "games:read")
					  .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "Auth0")
			);
			options.AddPolicy(Policies.WriteAccess, policy =>
				policy.RequireClaim("scope", "games:write")
					  .RequireRole("Admin")
					  .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "Auth0")
			);
		});

		return services;
	}
}
