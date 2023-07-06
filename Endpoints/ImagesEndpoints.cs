using GameStore.Api.Dtos;
using GameStore.Api.Interfaces;
using GameStore.Api.Utils;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameStore.Api.Endpoints;

public static class ImagesEndpoints
{
	public static RouteGroupBuilder MapImagesEndpoints(this IEndpointRouteBuilder routes)
	{
		var group = routes.NewVersionedApi()
						  .MapGroup("/images")
						  .HasApiVersion(1.0)
						  .WithParameterValidation()
						  .WithOpenApi()
						  .WithTags("Images");

		group.MapPost("/upload", async Task<Results<Ok<SuccessWithDataResponse<ImageUploadDto>>, BadRequest<FailResponse>>> (
			ICloudinaryImageUploader uploader,
			ILoggerFactory loggerFactory,
			IFormFile file) =>
		{
			if (file.Length <= 0) return Responses.BadRequest("File is empty.");

			var imageUrl = await uploader.ImageUploadAsync(file);
			loggerFactory.CreateLogger("CloudinaryImageUploader")
						 .LogInformation("Image uploaded to {imageUrl}", imageUrl);

			return Responses.SuccessWithData(new ImageUploadDto(imageUrl));
		})
		.MapToApiVersion(1.0)
		.RequireAuthorization(Policies.WriteAccess)
		.WithSummary("Upload image")
		.WithDescription("Upload image to cloudinary and get url to it.");

		return group;
	}
}
