namespace Jina.Domain.Multilingual;

public class GetMultilingualTopicRequest
{
    public int Id { get; set; }
}

public class GetMultilingualTopicResult
{
    public int Id { get; set; }
    public string PrimaryCultureType { get; set; }

    public List<MultilingualTopicConfigResult> MultilingualTopicConfigResults { get; set; }
}

public class MultilingualTopicConfigResult
{
    public int Id { get; set; }
    public string CultureType { get; set; }
    public int SortNo { get; set; }
}