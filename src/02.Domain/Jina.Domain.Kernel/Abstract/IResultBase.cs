namespace Jina.Domain.Kernel.Abstract;

public interface IResultBase
{
    List<string> Messages { get; set; }

    bool Succeeded { get; set; }
}

public interface IResultBase<out T> : IResultBase
{
    T Data { get; }
}