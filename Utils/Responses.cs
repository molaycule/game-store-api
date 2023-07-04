using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Utils;

public class SuccessResponse<T>
{
    public string Status { get; } = "success";
    public T Data { get; set; } = default!;
}

public class FailResponse
{
    public string Status { get; } = "fail";
    public string Message { get; set; } = default!;
}

public static class Responses
{
    public static Ok<SuccessResponse<T>> Success<T>(T data) =>
        TypedResults.Ok(new SuccessResponse<T> { Data = data });

    public static CreatedAtRoute<SuccessResponse<T>> Created<T>(string routeName, int id, T data) =>
        TypedResults.CreatedAtRoute(new SuccessResponse<T> { Data = data }, routeName, new { id });

    public static BadRequest<FailResponse> BadRequest(string message) =>
        TypedResults.BadRequest(new FailResponse { Message = message });

    public static NotFound<FailResponse> NotFound(string entityName, int id) =>
        TypedResults.NotFound(new FailResponse { Message = $"{entityName} with id {id} was not found" });

    public static IResult InternalServerError(InternalServerErrorParams errorParams)
    {
        var problem = new ProblemDetails
        {
            Title = "Internal Server Error",
            Detail = errorParams.Message,
            Status = StatusCodes.Status500InternalServerError,
            Extensions =
            {
                ["traceId"] = Activity.Current?.TraceId.ToString()
            }
        };

        if (errorParams.Environment?.IsDevelopment() ?? false)
            problem.Extensions["exception"] = errorParams.Exception?.ToString();

        errorParams.Logger?.LogError(
            errorParams.Exception, "{Message}. TraceId: {TraceId}", errorParams.Message, Activity.Current?.TraceId.ToString()
        );

        return Results.Problem(problem);
    }
}
