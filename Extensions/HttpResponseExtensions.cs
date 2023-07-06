using System.Text.Json;

namespace GameStore.Api.Extensions;

public static class HttpResponseExtensions
{
	public static void AddPaginationHeader(this HttpResponse response, int pageNumber, int pageSize, int totalCount)
	{
		var paginationHeader = new
		{
			currentPage = pageNumber,
			pageSize,
			totalCount,
			totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
		};

		response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationHeader));
	}
}
