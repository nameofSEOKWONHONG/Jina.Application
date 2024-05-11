namespace Jina.Domain;

public class RequestBase
{
    public string CreatedName { get; set; }
    public string LastModifiedName { get; set; }
    
    /// <summary>
    /// 질의시 캐시를 위한 값
    /// </summary>
    public DateTime RequestDate { get; private set; } = DateTime.Now;
}