using System.Diagnostics;
using GameStore.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Utils;

public static class Responses
{
    public static IResult Success<T>(T data) =>
        Results.Ok(new { status = "success", data });

    public static IResult Created<T>(string routeName, T data) where T : IEntity =>
        Results.CreatedAtRoute(routeName, new { id = data.Id }, new
        {
            status = "success",
            data
        });

    public static IResult NotFound(string entityName, int id) =>
        Results.NotFound(new { status = "fail", message = $"{entityName} with id {id} was not found" });

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
