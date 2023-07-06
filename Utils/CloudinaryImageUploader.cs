using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameStore.Api.Interfaces;

namespace GameStore.Api.Utils;

public class CloudinaryImageUploader : ICloudinaryImageUploader
{
	private readonly Cloudinary cloudinary;

	public CloudinaryImageUploader(IConfiguration configuration)
	{
		var cloudName = configuration.GetSection("Cloudinary:CloudName").Value;
		var apiKey = configuration.GetSection("Cloudinary:ApiKey").Value;
		var apiSecret = configuration.GetSection("Cloudinary:ApiSecret").Value;

		Account account = new(cloudName, apiKey, apiSecret);
		cloudinary = new(account);
		cloudinary.Api.Secure = true;
	}

	public async Task<string> ImageUploadAsync(IFormFile file)
	{
		var result = await cloudinary.CreateFolderAsync("GameStore");
		ImageUploadParams uploadParams = new()
		{
			File = new FileDescription(file.FileName, file.OpenReadStream()),
			Folder = result.Name,
		};
		var uploadResult = cloudinary.Upload(uploadParams);
		return uploadResult.SecureUrl.ToString();
	}
}
