﻿using System;

namespace Jina.Passion.Client.Base.Abstract.Interfaces
{
    public interface IManager : IDisposable
    {
    }

    public interface ITransientManager : IManager
    {
    }

    public interface IScopeManager : IManager
    {
    }

    public interface ISingletonManager : IManager
    {
    }
}