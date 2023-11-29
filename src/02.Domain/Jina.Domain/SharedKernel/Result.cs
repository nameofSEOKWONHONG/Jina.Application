using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.SharedKernel;

public class Result : IResultBase
{
    public Result()
    {
    }

    public List<string> Messages { get; set; } = new List<string>();

    public bool Succeeded { get; set; }

    public static IResultBase Fail()
    {
        return new Result { Succeeded = false };
    }

    public static IResultBase Fail(string message)
    {
        return new Result { Succeeded = false, Messages = new List<string> { message } };
    }

    public static IResultBase Fail(List<string> messages)
    {
        return new Result { Succeeded = false, Messages = messages };
    }

    public static Task<IResultBase> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public static Task<IResultBase> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static Task<IResultBase> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }

    public static IResultBase Success()
    {
        return new Result { Succeeded = true, Messages = new List<string>() { "Success." } };
    }

    public static IResultBase Success(string message)
    {
        return new Result { Succeeded = true, Messages = new List<string> { "Success.", message } };
    }

    public static Task<IResultBase> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<IResultBase> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }
}

public class Result<T> : Result, IResultBase<T>
{
    public Result()
    {
    }

    public T Data { get; set; }

    public new static Result<T> Fail()
    {
        return new Result<T> { Succeeded = false };
    }

    public new static Result<T> Fail(string message)
    {
        return new Result<T> { Succeeded = false, Messages = new List<string> { message } };
    }

    public new static Result<T> Fail(List<string> messages)
    {
        return new Result<T> { Succeeded = false, Messages = messages };
    }

    public new static Task<Result<T>> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public new static Task<Result<T>> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public new static Task<Result<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }

    public new static Result<T> Success()
    {
        return new Result<T> { Succeeded = true, Messages = new List<string>() { "Success." } };
    }

    public new static Result<T> Success(string message)
    {
        return new Result<T> { Succeeded = true, Messages = new List<string> { "Success.", message } };
    }

    public static Result<T> Success(T data)
    {
        return new Result<T> { Succeeded = true, Data = data, Messages = new List<string>() { "Success." } };
    }

    public static Result<T> Success(T data, string message)
    {
        return new Result<T> { Succeeded = true, Data = data, Messages = new List<string> { "Success.", message } };
    }

    public static Result<T> Success(T data, List<string> messages)
    {
        messages.Insert(0, "Search Success.");
        return new Result<T> { Succeeded = true, Data = data, Messages = messages };
    }

    public new static Task<Result<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public new static Task<Result<T>> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public static Task<Result<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<Result<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }
}