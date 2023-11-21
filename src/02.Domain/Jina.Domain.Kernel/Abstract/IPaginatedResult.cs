namespace Jina.Domain.SharedKernel.Abstract;

public interface IPaginatedResult : IResultBase
{
    int CurrentPage { get; }

    int TotalPages { get; }

    int TotalCount { get; }

    int PageSize { get; }

    bool HasPreviousPage { get; }

    bool HasNextPage { get; }
}