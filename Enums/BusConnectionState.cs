namespace IfKThenX.Enums;

/// <summary>
/// Represents the connection state of the bus.
/// </summary>
public enum BusConnectionState
{
    /// <summary>
    /// The connection to the bus interface is established and working.
    /// </summary>
    Connected,

    /// <summary>
    /// The connection to the bus interface is still closed or has been closed.
    /// </summary>
    Closed,

    /// <summary>
    /// The connection to the bus interface has been successfully opened, but there is a (temporary) communication problem.
    /// </summary>
    Broken,

    /// <summary>
    /// The connection to the bus interface is established and working, but the interface reports a failed connection.
    /// </summary>
    MediumFailure
}
