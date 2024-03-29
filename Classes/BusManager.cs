﻿using IfKThenX.Interfaces;
using Knx.Falcon;
using Knx.Falcon.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace IfKThenX;

public class BusManager : IBusManager
{
    private KnxBus? _bus;
    private IConnectionParametes? _connectionParameters;

    private IBusState _realBusState = new BusState();
    private List<IK> _ks = new List<IK>();
    private Enums.BusConnectionState _connectionState;

    public bool AutoReconnect { get; set; } = true;
    private int AutoreconnectDelay { get; set; } = 60_000;

    /// <inheritdoc/>
    public Enums.BusConnectionState ConnectionState { get => _connectionState;}

    /// <inheritdoc/>
    public event AsyncEventHandler<ITelegram>? ReceivedBusTelegram;

    /// <inheritdoc/>
    public event EventHandler<EventArgs>? BusConnectionStateChanged;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionParameters">The <see cref="IConnectionParametes"/> that are used to connect to the bus.</param>
    public BusManager(IConnectionParametes connectionParameters)
    {
        _connectionParameters = connectionParameters;
        NewFalconBus(_connectionParameters);
        AddMethodsToEvents();
    }

    private void NewFalconBus(IConnectionParametes connectionParameters)
    {
        _bus = new KnxBus(ConvertToFalcon.FromIConnectionParameters(connectionParameters));
    }

    private void RemoveFalconBus()
    {
        if (_bus is not null)
        {
            _bus.Dispose();
        }
        _bus= null;
    }

    private void AddMethodsToEvents()
    {
        if (_bus is not null)
        {
            _bus.GroupMessageReceived += OnGroupMessageReceivedFromFalcon;
            _bus.ConnectionStateChanged += OnConnectionStateChangedFromFalcon;
        }
        ReceivedBusTelegram += OnReceivedBusTelegramAsync;
        BusConnectionStateChanged += OnBusConnectionStateChanged;
    }

    private void RemoveMethodsFromEvents()
    {
        if (_bus is not null)
        {
            _bus.GroupMessageReceived -= OnGroupMessageReceivedFromFalcon;
            _bus.ConnectionStateChanged -= OnConnectionStateChangedFromFalcon;
        }
        ReceivedBusTelegram -= OnReceivedBusTelegramAsync;
        BusConnectionStateChanged -= OnBusConnectionStateChanged;
    }

    /// <inheritdoc/>
    public async Task ConnectAsync()
    {
        if (_bus is not null)
            await _bus.ConnectAsync();

    }

    private void OnGroupMessageReceivedFromFalcon(object? sender, GroupEventArgs args)
    {
        ITelegram telegram = ConvertFromFalcon.ToITelegram(args);
        ReceivedBusTelegram?.Invoke(this, telegram);
    }

    private void OnConnectionStateChangedFromFalcon(object? sender, EventArgs args)
    {
        if (_bus is not null)
        {
            _connectionState = ConvertFromFalcon.ToBusConnectionState(_bus.ConnectionState);
        }
        BusConnectionStateChanged?.Invoke(this, new EventArgs());
    }

    private void OnBusConnectionStateChanged(object? sender, EventArgs e)
    {
        Debug.WriteLine($"Connection changed to {ConnectionState}");
        if (AutoReconnect && ConnectionState == Enums.BusConnectionState.Closed)
        {
            _ = StartReconnectionProcess();
        }
    }

    private async Task OnReceivedBusTelegramAsync(object? sender, ITelegram telegram)
    {
        var stateOfGroup = new State(telegram.DestinationAddress)
        {
            Value = telegram.Value
        };

        _realBusState.AddOrUpdateState(stateOfGroup);

        Debug.WriteLine($"From: {telegram.SourceAddress} => {telegram.DestinationAddress}: {BitConverter.ToString(telegram.Value)}");
        await CheckForKAsync();
        CheckForKReset();
    }

    /// <summary>
    /// Checks if the current known state of the bus meets a k condition. If a condition is matched the corresponding x (can be more than one) are fired.
    /// </summary>
    private async Task CheckForKAsync()
    {
        IBusState realBusState = _realBusState;
        IEnumerable<IK> compareKs = _ks
            .Where(k => k.IsActive == true)
            .Where(k => k.HasFired == false);

        foreach (IK k in compareKs)
        {
            bool compareResult = Compare.BusStates(k.BusState, realBusState);
            if (compareResult == true)
            {
                await FireXAsync(k.Xs);

                k.HasFired = true;
            }
        }
    }

    /// <summary>
    /// Checks if states of any k is no longer matched with the bus state and reactivate the k.
    /// </summary>
    private void CheckForKReset()
    {
        IBusState realBusState = _realBusState;
        IEnumerable<IK> compareKs = _ks
            .Where(k => k.IsActive == true)
            .Where(k => k.HasFired == true);

        foreach (IK k in compareKs)
        {
            bool compareResult = Compare.BusStates(k.BusState, realBusState);
            if (compareResult == false && k.FireOnlyOnce == false)
            {
                k.HasFired = false;
            }
        }
    }

    /// <summary>
    /// Asynchronous fires all <see cref="IX"/> given as the parameter. Does not pay attention if the x is active.
    /// </summary>
    /// <param name="xs">A <see cref="List{IX}"/>. All underlying states of all members of this list are fired.</param>
    /// <returns>A <see cref="Task"/>, representing the firing process.</returns>
    private async Task FireXAsync(IReadOnlyList<IX> xs)
    {
        foreach (IX x in xs)
        {
            foreach (var s in x.BusState.States)
            {
                await WriteStateAsync(s).ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc/>
    /// <exception cref="NullReferenceException"></exception>
    public async Task WriteStateAsync(IState state)
    {
        if (state.Value is null)
            return;

        if (_bus is not null)
        {
            await _bus.WriteGroupValueAsync(new GroupAddress(state.Address), ConvertToFalcon.FromIState(state));
            return;
        }
        throw new NullReferenceException("You are not connected to a bus, or no bus object is initialized.");
    }

    /// <inheritdoc/>
    public void AddCondition(IK condition)
    {
        _ks.Add(condition);
    }

    /// <inheritdoc/>
    public void LoadBusGroups(IBusState newBusState)
    {
        _realBusState = newBusState;
    }

    /// <summary>
    /// Asynchronous requests all current values from the known bus representation. Every group address' values is requested successively.<br/>
    /// The timeout for every group is 3 seconds. Note, that this process can take every long.<br/>
    /// If the request succeeds, the value is written into the bus representation.
    /// </summary>
    /// <returns>A <see cref="Task"/>, representing the request process.</returns>
    public async Task GetAllStatesAsync(CancellationToken token = default(CancellationToken))
    {
        if (_bus is null)
            return;

        foreach (var state in _realBusState.States)
        {
            token.ThrowIfCancellationRequested();

            bool isReadable = await _bus.RequestGroupValueAsync(new GroupAddress(state.Address));
            if (isReadable)
            {
                Knx.Falcon.GroupValue valueFromFalcon = await _bus.ReadGroupValueAsync(state.Address);
                if (valueFromFalcon is not null)
                {
                    byte[] value = valueFromFalcon.Value;
                    state.Value = value;
                }
                if (state.Value is not null)
                    Debug.WriteLineIf(valueFromFalcon is not null, $"Read from {state.Address}: {BitConverter.ToString(state.Value)}");
            }
        }
    }

    private async Task Reconnect()
    {
        RemoveMethodsFromEvents();
        RemoveFalconBus();

        if (_connectionParameters is null)
            throw new NullReferenceException("No connection parameters provided!");

        NewFalconBus(_connectionParameters);
        AddMethodsToEvents();
        await ConnectAsync();
    }

    private async Task StartReconnectionProcess()
    {
        int timeLeft = AutoreconnectDelay;
        while (timeLeft > 0)
        {
            Debug.WriteLine($"Reconnecting in {timeLeft / 1000} seconds.");
            await Task.Delay(1000);
            timeLeft -= 1000;
        }
        Debug.WriteLine("Reconnecting...");
        try
        {
            await Reconnect();
        }
        catch (Exception ex)
        {
            await StartReconnectionProcess();
        }
    }
}
