namespace Jina.Domain.Shared;

public class PaginatedRequest
{
    /// <summary>
    /// 날짜 선택 모드
    /// </summary>
    public int SelectedMode { get; set; } = 1;

    public int PageNo { get; set; }
    public int PageSize { get; set; }

    public string SortName { get; set; }
    public string OrderBy { get; set; } = "DESC";

    public bool IsActive { get; set; } = true;
}

public class PaginatedRequest<T> : PaginatedRequest
{
    public T SearchData { get; set; }

    public PaginatedRequest()
    {
    }
}