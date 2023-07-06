using System.Diagnostics;

namespace GameStore.Api.Middleware;

public class RequestTimingMiddleware
{
	private readonly RequestDelegate next;
	private readonly ILogger<RequestTimingMiddleware> logger;

	public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
	{
		this.next = next;
		this.logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var stopwatch = Stopwatch.StartNew();
		try
		{
			await next(context);
		}
		finally
		{
			stopwatch.Stop();
			logger.LogInformation(
				"Request {Method} {Path} took {ElapsedMilliseconds}ms",
				context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds
			);
		}
	}
}
