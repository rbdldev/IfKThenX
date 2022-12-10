using System.Collections.Generic;

namespace IfKThenX.Interfaces;

public interface IK
{
    /// <summary>
    /// A subset of a <see cref="IBusState"/>, that is the base for deciding if a condition is reached.
    /// </summary>
    IBusState BusState { get; }

    /// <summary>
    /// If this condition is currently active/in use.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// If the condition was activated and has fired.
    /// </summary>
    bool HasFired { get; set; }

    /// <summary>
    /// If the conditions' x can only be fired once during the hole runtime.
    /// </summary>
    bool FireOnlyOnce { get; }

    /// <summary>
    /// The commands that are fired to the bus, wrapped in a <see cref="IX"/> list.
    /// </summary>
    IReadOnlyList<IX> Xs { get; }
}
