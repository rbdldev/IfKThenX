using System;
using System.Threading.Tasks;
using IfKThenX.Enums;

namespace IfKThenX.Interfaces;

public interface IBusManager
{
    /// <summary>
    /// Asynchronous connects to the bus.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the connection process.</returns>
    Task ConnectAsync();

    /// <summary>
    /// Gets information about the current state of the connection.
    /// </summary>
    /// <remarks>Any change in this value is reported by the <see cref="IfKThenX.Interfaces.IBusManager.BusConnectionStateChanged"/>  event.</remarks>
    BusConnectionState ConnectionState { get; }

    /// <summary>
    /// This event is raised whenever <see cref="IBusManager.ConnectionState"/> changed.
    /// </summary>
    event EventHandler<EventArgs>? BusConnectionStateChanged;

    /// <summary>
    /// This event is raised when a telegram is received from the bus.
    /// </summary>
    event AsyncEventHandler<ITelegram>? ReceivedBusTelegram;

    /// <summary>
    /// Adds a new - if k then x - condition.
    /// </summary>
    /// <param name="condition">The <see cref="IK"/> condition that contains the corresponding <see cref="IX"/> states that are fired if the condition is matched.</param>
    void AddCondition(IK condition);

    /// <summary>
    /// Manually loads a given <see cref="IBusState"/>, as the assumed bus representation. Overrides the previous state.
    /// </summary>
    /// <param name="newBusState">A <see cref="IBusState"/> that used as the new actual bus representation.</param>
    void LoadBusGroups(IBusState newBusState);

    /// <summary>
    /// Asynchronous sends a telegram to the connected bus, corresponding to the values given in the <see cref="IState"/> parameter.
    /// </summary>
    /// <param name="state">An <see cref="IState"/> objects, whichs parameters are used to generate the telegram thats send to the connected bus.</param>
    /// <returns>A <see cref="Task"/>, representing the telegrams sending process.</returns>
    Task WriteStateAsync(IState state);
}
