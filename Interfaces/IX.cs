using System.Collections.Generic;

namespace IfKThenX.Interfaces;

public interface IX
{
    /// <summary>
    /// A subset of a <see cref="IBusState"/>, that is fired when the corresponding k is activated.
    /// </summary>
    IBusState BusState { get; }

    /// <summary>
    /// If this condition is currently active/in use.
    /// </summary>
    bool IsActive { get; set; }
}
