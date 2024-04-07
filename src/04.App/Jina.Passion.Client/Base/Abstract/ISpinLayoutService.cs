using System;

namespace Jina.Passion.Client.Base.Abstract;

public interface ISpinLayoutService
{
    bool Loading { get; set; }

    void ShowProgress();

    void CloseProgress();

    void OnStateChange(Action action);
}