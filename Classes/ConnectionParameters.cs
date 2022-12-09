using IfKThenX.Interfaces;

namespace IfKThenX;
/// <summary>
/// Represents the parameters that are used to connect to the bus.
/// </summary>
public class ConnectionParameters : IConnectionParametes
{
    // <inheritdoc />
    public string IpAdress { get; }

    // <inheritdoc />
    public int Port { get; } = 3671;

    public ConnectionParameters(string ipAddress)
    {
        IpAdress= ipAddress;
    }

    public ConnectionParameters(string ipAddress, int port)
    {
        IpAdress = ipAddress;
        Port = port;
    }
}
