namespace GameStore.Api.Interfaces;

public interface ICloudinaryImageUploader
{
    Task<string> ImageUploadAsync(IFormFile file);
}