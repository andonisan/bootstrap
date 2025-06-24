using System.Diagnostics.CodeAnalysis;

namespace BuildingBlocks.Domain;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);

    public static Result Failure(Error[]? toArray)
    {
        
        if (toArray is null || toArray.Length == 0)
        {
            return Success();
        }

        if (toArray.Length == 1)
        {
            return Failure(toArray[0]);
        }

        var combinedError = new Error(
            string.Join(", ", toArray.Select(e => e.Code)),
            string.Join("; ", toArray.Select(e => e.Description)),
            ErrorType.Failure);

        return Failure(combinedError);
    }
}

public class Result<TValue>(TValue? value, bool isSuccess, Error error) : Result(isSuccess, error)
{
    [NotNull]
    public TValue Value => IsSuccess
        ? value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    
}
