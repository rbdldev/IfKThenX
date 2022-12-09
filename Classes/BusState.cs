using IfKThenX.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IfKThenX;

/// <summary>
/// Represents the state of a bus in the sense that a defined number of groups have a specific value.
/// </summary>
public class BusState : IBusState
{
    private List<IState> _states = new List<IState>();
    public IReadOnlyList<IState> States
    {
        get
        {
            return _states.AsReadOnly();
        }
    }

    /// <summary>
    /// Adds a state. If the corresponding group is already known, the value of the known group is updated.
    /// </summary>
    /// <param name="state"></param>
    public void AddOrUpdateState(IState state)
    {
        IState? knownState = _states.FirstOrDefault(s => s.Address == state.Address);
        if (knownState is not null)
        {
            knownState.Value = state.Value;
        }
        else
        {
            _states.Add(state);
        }
    }
}
