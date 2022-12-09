using IfKThenX.Interfaces;

namespace IfKThenX;

internal class Telegram : ITelegram
{
    /// <summary>
    /// The group address from where this telegram was sent.
    /// </summary>
    public string SourceAddress { get; }

    /// <summary>
    /// The destination group address of the telegram.
    /// </summary>
    public string DestinationAddress { get; }

    /// <summary>
    /// The value of the telegram.
    /// </summary>
    public byte[] Value { get; }

    /// <summary>
    /// If this telegram was sent with KNX Secure technology.
    /// </summary>
    public bool IsSecure { get; } = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceAddress">The group address from where this telegram was sent.</param>
    /// <param name="destinationAddress">The destination group address of the telegram.</param>
    /// <param name="value">The value of the telegram.</param>
    /// <param name="isSecure">If this telegram was sent with KNX Secure technology.</param>
    public Telegram(string sourceAddress, string destinationAddress, byte[] value, bool isSecure = false)
    {
        SourceAddress = sourceAddress;
        DestinationAddress = destinationAddress;
        Value = value;
        IsSecure = isSecure;
    }
}
