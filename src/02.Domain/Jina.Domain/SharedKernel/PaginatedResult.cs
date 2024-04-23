using Jina.Domain.SharedKernel.Abstract;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Jina.Domain.SharedKernel;

public class PaginatedResult<T> : IPaginatedResult
{
    public PaginatedResult(List<T> data)
    {
        Data = data;
    }

    public List<T> Data { get; set; }

    internal PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
    {
        Data = data;
        PageNo = page;
        Succeeded = succeeded;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public static PaginatedResult<T> Fail(string message)
    {
        return Fail(new List<string>() { message });
    }

    public static PaginatedResult<T> Fail(List<string> messages)
    {
        return new PaginatedResult<T>(false, default, messages);
    }

    public static Task<PaginatedResult<T>> FailAsync(string message)
    {
        return FailAsync(new List<string>() { message });
    }

    public static Task<PaginatedResult<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(new PaginatedResult<T>(false, default, messages));
    }

    public static PaginatedResult<T> Success(List<T> data, int totalCount, int currentPage, int pageSize)
    {
        var result = new PaginatedResult<T>(true, data, null, totalCount, currentPage, pageSize);
        result.Messages = new List<string>() { "Success." };
        return result;
    }

    public static Task<PaginatedResult<T>> SuccessAsync(List<T> data, int totalCount, int currentPage,
        int pageSize)
    {
        return Task.FromResult(Success(data, totalCount, currentPage, pageSize));
    }

    public int PageNo { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public bool HasPreviousPage => PageNo > 1;

    public bool HasNextPage => PageNo < TotalPages;

    public List<string> Messages { get; set; } = new List<string>();
    
    public Dictionary<string, string> ValidateErrors { get; set; }

    public bool Succeeded { get; set; }
}