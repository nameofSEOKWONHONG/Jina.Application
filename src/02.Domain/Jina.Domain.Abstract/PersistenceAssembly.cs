﻿using System.Reflection;

namespace Jina.Domain.Abstract;

/// <summary>
/// Represents the persistence assembly.
/// </summary>
public static class PersistenceAssembly
{
    /// <summary>
    /// Gets the persistence assembly.
    /// </summary>
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
}