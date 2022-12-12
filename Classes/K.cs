using IfKThenX.Interfaces;
using System;
using System.Collections.Generic;

namespace IfKThenX;

public class K : IK
{
    private bool _fireOnlyOnce = false;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IBusState BusState { get; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool HasFired { get; set; } = false;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool FireOnlyOnce { get => _fireOnlyOnce; }

    private List<IX> xs = new List<IX>();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IReadOnlyList<IX> Xs
    {
        get
        {
            return xs.AsReadOnly();
        }
    }

    public K(IBusState busState)
    {
        BusState = busState;
    }

    public K(IBusState busState, IX x)
    {
        ArgumentNullException.ThrowIfNull(x);
        BusState = busState;
        xs.Add(x);
    }

    public K(IBusState busState, IX x, bool fireOnlyOnce)
    {
        ArgumentNullException.ThrowIfNull(x);
        BusState = busState;
        xs.Add(x);
        _fireOnlyOnce= fireOnlyOnce;
    }

    /// <summary>
    /// Adds a new x to the ks' condition.
    /// </summary>
    /// <param name="x"></param>
    public void AddX(IX x)
    {
        xs.Add(x);
    }
}
