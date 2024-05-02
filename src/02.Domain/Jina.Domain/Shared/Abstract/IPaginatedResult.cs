namespace Jina.Domain.Shared.Abstract;

public interface IPaginatedResult : IResults
{
    int PageNo { get; }

    int TotalPages { get; }

    int TotalCount { get; }

    int PageSize { get; }

    bool HasPreviousPage { get; }

    bool HasNextPage { get; }
}