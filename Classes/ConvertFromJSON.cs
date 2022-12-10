using IfKThenX.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace IfKThenX;

/// <summary>
/// Provides usefull methods to convert from JSON to objects used in IfKThenX.
/// </summary>
public class ConvertFromJSON
{
    /// <summary>
    /// Converts from an JSON file to a list of <see cref="IState"/> objects.
    /// </summary>
    /// <param name="filePath">The absolute path to the JSON file.</param>
    /// <returns>A <see cref="Task"/> representing the process of generating the list of <see cref="IState"/>.</returns>
    public static async Task<List<IState>> ToIStatesAsync(string filePath)
    {
        using FileStream openStream = File.OpenRead(filePath);
        List<State>? parsedStates = await JsonSerializer.DeserializeAsync<List<State>>(openStream);
        
        if (parsedStates is null)
            return new List<IState>();
        
        return new List<IState>(parsedStates);
    }

    /// <summary>
    /// Converts from an JSON file to an <see cref="IBusState"/>.
    /// </summary>
    /// <param name="filePath">The absolute path to the JSON file.</param>
    /// <returns>A <see cref="Task"/> representing the process of generating the <see cref="IBusState"/>.</returns>
    public static async Task<IBusState> ToIBusStateAsync(string filePath)
    {
        List<IState> states = await ToIStatesAsync(filePath);
        var busState = new BusState();
        foreach (var state in states)
        {
            busState.AddOrUpdateState(state);
        }
        return busState;
    }
}
