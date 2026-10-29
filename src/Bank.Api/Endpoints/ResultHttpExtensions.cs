using Bank.Api.Common;

namespace Bank.Api.Endpoints;

/// <summary>
/// Result tipini HTTP yanıtına çevirir. Hatalar RFC 7807 ProblemDetails formatında döner.
/// </summary>
public static class ResultHttpExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult>? onSuccess = null)
    {
        if (result.IsSuccess)
        {
            return onSuccess is null ? Results.Ok(result.Value) : onSuccess(result.Value!);
        }

        var error = result.Error!;
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status422UnprocessableEntity
        };

        return Results.Problem(
            statusCode: statusCode,
            title: error.Code,
            detail: error.Message);
    }
}
