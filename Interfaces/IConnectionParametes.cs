namespace IfKThenX.Interfaces;

/// <summary>
/// Objects that represents the parameters that are used to connect to the bus have to implement this interface.
/// </summary>
public interface IConnectionParametes
{
    /// <summary>
    /// The ip address of the bus interface.
    /// </summary>
    string IpAdress { get; }

    /// <summary>
    /// The port where the bus IP interface is reachable.
    /// </summary>
    int Port { get; }
}
