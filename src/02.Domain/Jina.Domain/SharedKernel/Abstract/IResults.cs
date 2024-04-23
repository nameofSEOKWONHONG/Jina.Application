namespace Jina.Domain.SharedKernel.Abstract;

public interface IResultBase
{
    List<string> Messages { get; set; }
    
    Dictionary<string, string> ValidateErrors { get; set; }

    bool Succeeded { get; set; }
}

public interface IResults<out T> : IResultBase
{
    T Data { get; }
}