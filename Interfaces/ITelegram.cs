namespace IfKThenX.Interfaces;

public interface ITelegram
{
    /// <summary>
    /// The physical address of the sender.
    /// </summary>
    string SourceAddress { get; }

    /// <summary>
    /// The group address the telegram is determined for.
    /// </summary>
    string DestinationAddress { get; }

    /// <summary>
    /// The value that was send from or to the group.
    /// </summary>
    byte[] Value { get; }

    /// <summary>
    /// Indicates whether the telegram is secured.
    /// </summary>
    bool IsSecure { get; }
}
