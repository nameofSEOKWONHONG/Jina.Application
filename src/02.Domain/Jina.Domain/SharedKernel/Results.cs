using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.SharedKernel;

public class Results : IResultBase
{
    public List<string> Messages { get; set; } = new List<string>();

    public Dictionary<string, string> ValidateErrors { get; set; }
        = new Dictionary<string, string>();

    public Results()
    {
    }

    public bool Succeeded { get; set; }

    public static IResultBase Fail()
    {
        return new Results { Succeeded = false };
    }

    public static IResultBase Fail(string message)
    {
        return new Results { Succeeded = false, Messages = [message] };
    }

    public static IResultBase Fail(List<string> messages)
    {
        return new Results { Succeeded = false, Messages = messages };
    }

    public static IResultBase Fail(Dictionary<string, string> errors)
    {
        return new Results() { Succeeded = false, ValidateErrors = errors };
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

    public static Task<IResultBase> FailAsync(Dictionary<string, string> errors)
    {
        return Task.FromResult(Fail(errors));
    }

    public static IResultBase Success()
    {
        return new Results { Succeeded = true, Messages = ["Success."] };
    }

    public static IResultBase Success(string message)
    {
        return new Results { Succeeded = true, Messages = ["Success.", message] };
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

public class Results<T> : Results, IResults<T>
{
    public Results()
    {
    }

    public T Data { get; set; }

    public new static Results<T> Fail()
    {
        return new Results<T> { Succeeded = false };
    }

    public new static Results<T> Fail(string message)
    {
        return new Results<T> { Succeeded = false, Messages = [message] };
    }

    public new static Results<T> Fail(List<string> messages)
    {
        return new Results<T> { Succeeded = false, Messages = messages };
    }

    public new static Results<T> Fail(Dictionary<string, string> errors)
    {
        return new Results<T>() { Succeeded = false, ValidateErrors = errors };
    }

    public new static Task<Results<T>> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public new static Task<Results<T>> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public new static Task<Results<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }

    public new static Task<Results<T>> FailAsync(Dictionary<string, string> errors)
    {
        return Task.FromResult(Fail(errors));
    }

    public new static Results<T> Success()
    {
        return new Results<T> { Succeeded = true, Messages = ["Success."] };
    }

    public new static Results<T> Success(string message)
    {
        return new Results<T> { Succeeded = true, Messages = ["Success.", message] };
    }

    public static Results<T> Success(T data)
    {
        return new Results<T> { Succeeded = true, Data = data, Messages = ["Success."] };
    }

    public static Results<T> Success(T data, string message)
    {
        return new Results<T> { Succeeded = true, Data = data, Messages = ["Success.", message] };
    }

    public static Results<T> Success(T data, List<string> messages)
    {
        messages.Insert(0, "Search Success.");
        return new Results<T> { Succeeded = true, Data = data, Messages = messages };
    }

    public new static Task<Results<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public new static Task<Results<T>> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public static Task<Results<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<Results<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }
}