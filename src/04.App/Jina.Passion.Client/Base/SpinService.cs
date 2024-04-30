using Jina.Passion.Client.Base.Abstract;

namespace Jina.Passion.Client.Base;

public class SpinService : ISpinService
{
    public bool Loading { get; private set; }
    
    public void Show()
    {
        Loading = true;
    }

    public void Close()
    {
        Loading = false;
    }

    public void OnStateChange(Action action)
    {
        
    }
}