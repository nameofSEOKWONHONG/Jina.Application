using System;

namespace Jina.Passion.Client.Base.Abstract;

public interface ISpinService
{
    bool Loading { get; }

    void Show();

    void Close();

    void OnStateChange(Action action);
}