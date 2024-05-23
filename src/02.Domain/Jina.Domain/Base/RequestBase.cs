namespace Jina.Domain;

public class RequestBase
{
    public string CreatedName { get; set; }
    public string LastModifiedName { get; set; }
    
    /// <summary>
    /// Manual tag
    /// 서버측 질의시 캐시에 사용될 값.
    /// post, put, update 후에 공통적으로 MTag반환도록 해야 함.
    /// get 질의시 MTag가 포함되도록 해야 함.
    /// </summary>
    public string MTag { get; private set; }
}