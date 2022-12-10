using IfKThenX.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace IfKThenX;

public class ConvertFromJSON
{
    public static async Task<List<IState>> ToIStatesAsync(string filePath)
    {
        using FileStream openStream = File.OpenRead(filePath);
        List<State>? parsedStates = await JsonSerializer.DeserializeAsync<List<State>>(openStream);
        
        if (parsedStates is null)
            return new List<IState>();
        
        return new List<IState>(parsedStates);
    }

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
