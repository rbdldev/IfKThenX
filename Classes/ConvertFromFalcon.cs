using IfKThenX.Interfaces;
using Knx.Falcon;
using System;

namespace IfKThenX;

/// <summary>
/// Provides usefull methods to convert objects from the Knx.Falkon Sdk to objects used in IfKThenX.
/// </summary>
internal static class ConvertFromFalcon
{
    /// <summary>
    /// Converts <see cref="GroupEventArgs"/> to <see cref="ITelegram"/>.
    /// </summary>
    /// <param name="args"></param>
    /// <returns>An <see cref="ITelegram"/> object.</returns>
    public static ITelegram ToITelegram(GroupEventArgs args)
    {
        return new Telegram(
            sourceAddress: args.SourceAddress,
            destinationAddress: args.DestinationAddress,
            value: args.Value.Value,
            isSecure: args.IsSecure
            );
    }

    /// <summary>
    /// Converts <see cref="Knx.Falcon.BusConnectionState"/> to <see cref="IfKThenX.Enums.BusConnectionState"/>.
    /// </summary>
    /// <param name="state"></param>
    /// <returns>An <see cref="IfKThenX.Enums.BusConnectionState"/> enum.</returns>
    public static Enums.BusConnectionState ToBusConnectionState(Knx.Falcon.BusConnectionState state)
    {
        return state switch
        {
            Knx.Falcon.BusConnectionState.Connected => Enums.BusConnectionState.Connected,
            Knx.Falcon.BusConnectionState.Closed => Enums.BusConnectionState.Closed,
            Knx.Falcon.BusConnectionState.Broken => Enums.BusConnectionState.Broken,
            Knx.Falcon.BusConnectionState.MediumFailure => Enums.BusConnectionState.MediumFailure,
            _ => Enums.BusConnectionState.Broken,
        };
    }


    public static IfKThenX.DataPointType? ToDPT(string? dpt)
    {
        if (dpt is null)
            return null;

        int indexFirstDash = dpt.IndexOf('-');
        int indexSecondDash = dpt.LastIndexOf('-');
        string currentDpt = dpt.Substring(indexFirstDash + 1, indexSecondDash - indexFirstDash - 1);

        int parsedDpt;
        if (Int32.TryParse(currentDpt, out parsedDpt))
        {
            return (IfKThenX.DataPointType)parsedDpt;
        }
        return null;
    }
}
