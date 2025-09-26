using FluentResults;

namespace SasiadMa.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToIResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors.Select(e => new { Code = e.Message, Message = e.Metadata?.Values.FirstOrDefault()?.ToString() ?? e.Message }));
    }

    public static IResult ToIResult(this Result result)
    {
        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result.Errors.Select(e => new { Code = e.Message, Message = e.Metadata?.Values.FirstOrDefault()?.ToString() ?? e.Message }));
    }
}