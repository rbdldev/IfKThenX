using IfKThenX.Interfaces;
using System.Linq;

namespace IfKThenX;

/// <summary>
/// Provides usefull methods to compare IfKThenX objects.
/// </summary>
internal static class Compare
{
    /// <summary>
    /// Compares two <see cref="IBusState"/>.<br/>
    /// Matches if all states from the <see cref="IBusState"/> with less entries are found with the same value in the <see cref="IBusState"/> with more entries.<br/>
    /// The order of the states in the parameters does not matter.
    /// </summary>
    /// <param name="state1">The first state to compare.</param>
    /// <param name="state2">The second state that is compared first the first one.</param>
    /// <returns>A <see cref="bool"/>, wether all conditions from one <see cref="IBusState"/> are found in the other one.</returns>
    public static bool BusStates(IBusState state1, IBusState state2)
    {
        IBusState stateWithMostEntries;
        IBusState stateWithLessEntries;

        if (state1.States.Count > state2.States.Count)
        {
            stateWithMostEntries = state1;
            stateWithLessEntries = state2;
        }
        else
        {
            stateWithMostEntries = state2;
            stateWithLessEntries = state1;
        }

        bool[] comparingResults = new bool[stateWithLessEntries.States.Count];
        
        int i = 0;
        foreach (var comState in stateWithLessEntries.States)
        {
            IState? stateFound = stateWithMostEntries.States.FirstOrDefault(s => s.Address == comState.Address);

            if (stateFound is null)
                return false;

            if (stateFound.Value is null || comState.Value is null)
                return false;

            if (stateFound.Value.SequenceEqual(comState.Value))
            {
                comparingResults[i++] = true;
            }
        }
        return comparingResults.All(r => r == true);
    }
}
