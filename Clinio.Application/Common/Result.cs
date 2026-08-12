namespace Clinio.Application.Common;

public record ResultError(string Code, string Description);
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public ResultError? Error { get; }

    protected Result(bool isSuccess, T? value, ResultError? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(ResultError error) => new(false, default, error);
}