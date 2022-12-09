using IfKThenX.Interfaces;
using Knx.Falcon;
using Knx.Falcon.Configuration;
using System;

namespace IfKThenX;

/// <summary>
/// Provides usefull methods to convert objects from IfKThenX to the Knx.Falkon Sdk.
/// </summary>
public static class ConvertToFalcon
{
    /// <summary>
    /// Converts <see cref="IConnectionParametes"/> to <see cref="IpTunnelingConnectorParameters"/>.
    /// </summary>
    /// <param name="connectionParameters"></param>
    /// <returns>An <see cref="IpTunnelingConnectorParameters"/> object.</returns>
    public static IpTunnelingConnectorParameters FromIConnectionParameters(IConnectionParametes connectionParameters)
    {
        return new IpTunnelingConnectorParameters
            (
            ipAddressOrHostName: connectionParameters.IpAdress,
            ipPort: connectionParameters.Port
            );
    }

    /// <summary>
    /// Converts an <see cref="IState"/> objects value to a <see cref="GroupValue"/> of the Falcon Sdk.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static GroupValue FromIState(IState state)
    {
        ArgumentNullException.ThrowIfNull(state.Value);
        return state.DataPointType switch
        {
            DataPointType.Dpt1 => new GroupValue(BitConverter.ToBoolean(state.Value, 0)),
            DataPointType.Dpt2 => new GroupValue(state.Value[0], 2),
            DataPointType.Dpt3 => new GroupValue(state.Value[0], 4),
            DataPointType.Dpt4 or DataPointType.Dpt5 or DataPointType.Dpt6 => new GroupValue(state.Value[0]),
            _ => new GroupValue(state.Value),
        };
    }
}
