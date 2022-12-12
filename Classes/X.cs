using IfKThenX.Interfaces;

namespace IfKThenX;

public class X : IX
{
    private IBusState _busState = new BusState();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IBusState BusState
    {
        get
        {
            return _busState;
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool IsActive { get; set; } = true;


    public X()
    {
    }

    public X(IBusState busState)
    {
        _busState = busState;
    }

    /// <summary>
    /// Adds new state to the underlying busstate that is activated when this x is fired.
    /// </summary>
    /// <param name="state"></param>
    public void AddState(IState state)
    {
        _busState.AddOrUpdateState(state);
    }

    /// <summary>
    /// Adds all states to the underlying busstate from the injected parameter.
    /// </summary>
    /// <param name="busState"></param>
    public void AppendBusState(IBusState busState)
    {
        foreach (IState state in busState.States)
        {
            _busState.AddOrUpdateState(state);
        }
    }

    /// <summary>
    /// Set the underlying bus state that is activated when this x is fired. Overrides the existing bus state.
    /// </summary>
    /// <param name="busState"></param>
    public void SetBusState(IBusState busState)
    {
        _busState = busState;
    }
}
