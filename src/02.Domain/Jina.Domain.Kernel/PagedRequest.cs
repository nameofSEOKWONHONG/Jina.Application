namespace Jina.Domain.SharedKernel;

public abstract class PagedRequest
{
    /// <summary>
    /// 날짜 선택 모드
    /// </summary>
    public int SelectedMode { get; set; } = 1;

    /// <summary>
    /// 검색 시작일
    /// </summary>
    public DateTime? From { get; set; }

    /// <summary>
    /// 검색 종료일
    /// </summary>
    public DateTime? To { get; set; }

    public int PageNo { get; set; }
    public int PageSize { get; set; }

    public string SortName { get; set; }
    public string OrderBy { get; set; } = "DESC";

    public bool IsActive { get; set; } = true;
}

public abstract class PagedRequest<T> : PagedRequest
{
    public T SearchData { get; set; }
}