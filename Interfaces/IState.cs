namespace IfKThenX.Interfaces;

public interface IState
{
    /// <summary>
    /// The name of the group this state corresponds to.
    /// </summary>
    string? GroupName { get; }

    /// <summary>
    /// The group address of the group this state corresponds to.
    /// </summary>
    string Address { get; }

    /// <summary>
    /// The value of the group this state corresponds to. Only applicable if it is a group with status.
    /// </summary>
    byte[]? Value { get; set; }

    /// <summary>
    /// The type of data point this states' value is corresponding to.
    /// </summary>
    DataPointType DataPointType { get; }
}
