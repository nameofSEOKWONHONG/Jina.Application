namespace Jina.Domain.Shared.Abstract;

public interface IResults
{
    List<string> Messages { get; set; }
    
    Dictionary<string, string> ValidateErrors { get; set; }

    bool Succeeded { get; set; }
}

public interface IResults<out T> : IResults
{
    T Data { get; }
}