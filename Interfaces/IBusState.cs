using System.Collections.Generic;

namespace IfKThenX.Interfaces;

public interface IBusState
{
    /// <summary>
    /// All states the bus contains.
    /// </summary>
    IReadOnlyList<IState> States { get; }

    /// <summary>
    /// Adds new states. If a state is already known, the known state is updated.
    /// </summary>
    /// <param name="state"></param>
    public void AddOrUpdateState(IState state);
}
