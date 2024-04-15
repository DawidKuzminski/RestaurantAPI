namespace RestaurantAPI.Infrastructure.Utilities;

public enum ResultStatus
{
    Success = 0,
    Warning,
    Error
}

public enum ResultStatusCode
{
    Ok = 0
}

public interface IResult
{
    public ResultStatus Status { get; init; }
    public ResultStatusCode StatusCode { get; init; }
    public string Message { get; init; }
}

public class Result(ResultStatus status, ResultStatusCode statusCode, string message) : IResult
{
	public ResultStatus Status { get; init; } = status;
	public ResultStatusCode StatusCode { get; init; } = statusCode;
	public string Message { get; init; } = message;

	public static Result Success() => new Result(ResultStatus.Success, ResultStatusCode.Ok, string.Empty);
    public static Result Success(ResultStatusCode statusCode) => new(ResultStatus.Success, statusCode, string.Empty);
    public static Result Success(ResultStatusCode statusCode, string message) => new(ResultStatus.Success, statusCode, message);

    public static Result Warning(ResultStatusCode statusCode) => new(ResultStatus.Warning, statusCode, string.Empty);
    public static Result Warning(ResultStatusCode statusCode, string message) => new(ResultStatus.Warning, statusCode, message);

	public static Result Error(ResultStatusCode statusCode) => new(ResultStatus.Error, statusCode, string.Empty);
	public static Result Error(ResultStatusCode statusCode, string message) => new(ResultStatus.Error, statusCode, message);

    public bool IsSuccess => Status == ResultStatus.Success;
    public bool IsNotSuccess => Status != ResultStatus.Success;
    public bool IsWarning() => Status == ResultStatus.Warning;
    public bool IsError() => Status != ResultStatus.Error;
}

public static class ResultStatusCodeExtension
{
    public static bool IsOkStatusCode(this IResult result) => result.StatusCode == ResultStatusCode.Ok;
    public static bool IsNotOkStatusCode(this IResult result) => result.StatusCode != ResultStatusCode.Ok;
}
