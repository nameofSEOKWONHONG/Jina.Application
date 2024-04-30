using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Shared;

public interface ICursorResult : IResultBase
{
    int Cursor { get; }

    int PageSize { get; }
}

public class CursorResult<T> : ICursorResult 
{
    public int Cursor { get; }
    public int PageSize { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
    public List<string> Messages { get; set; }
    public Dictionary<string, string> ValidateErrors { get; set; }
    public bool Succeeded { get; set; }    
    
    public List<T> Data { get; set; }
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="data"></param>
    public CursorResult(List<T> data)
    {
        this.Data = data;
    }

    internal CursorResult(bool succeeded, List<T> data = default, List<string> messages = null,
        int cursor = 0, int pageSize = 0)
    {
        Data = data;
        Cursor = cursor;
        Succeeded = succeeded;
        PageSize = pageSize;
    }
    
    public static CursorResult<T> Fail(string message)
    {
        return Fail(new List<string>() { message });
    }

    public static CursorResult<T> Fail(List<string> messages)
    {
        return new CursorResult<T>(false, default, messages);
    }

    public static Task<CursorResult<T>> FailAsync(string message)
    {
        return FailAsync(new List<string>() { message });
    }

    public static Task<CursorResult<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(new CursorResult<T>(false, default, messages));
    }

    public static CursorResult<T> Success(List<T> data, int cursor, int pageSize)
    {
        var result = new CursorResult<T>(true, data, null, cursor, pageSize);
        result.Messages = new List<string>() { "Success." };
        return result;
    }

    public static Task<CursorResult<T>> SuccessAsync(List<T> data, int currentCursor,
        int pageSize)
    {
        return Task.FromResult(Success(data, currentCursor, pageSize));
    }
}