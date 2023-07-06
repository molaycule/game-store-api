using GameStore.Api.Interfaces;
using GameStore.Api.Utils;

namespace GameStore.Api.Extensions;

public static class CloudinaryExtensions
{
	public static IServiceCollection AddCloudinaryService(this IServiceCollection services, IConfiguration configuration) =>
		services.AddSingleton<ICloudinaryImageUploader>(new CloudinaryImageUploader(configuration));
}
