namespace Jina.Domain.Multilingual;

public class GetMultilingualConfigResult
{
    public int Id { get; set; }
    
    public string LanguageCode { get; set; }
    
    public int Sort { get; set; }
    
    public int? ParentId { get; set; }
    
    public List<GetMultilingualConfigResult> ChildContents { get; set; }
}