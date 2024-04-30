namespace Jina.Domain;

public class CursorRequest
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

    public int Cursor { get; set; }
    public int PageSize { get; set; }

    public string SortName { get; set; }
    public string OrderBy { get; set; } = "DESC";

    public bool IsActive { get; set; } = true;
}

public class CursorRequest<T> : CursorRequest
{
    public T SearchData { get; set; }

    public CursorRequest()
    {
    }
}