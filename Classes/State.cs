﻿using IfKThenX.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace IfKThenX;

public class State : IState
{
    /// <summary>
    /// The name of the group the states corresponds to.
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// The group address of the group the state corresponds to.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// The value of the state. It null if no value is known or the states corresponds to a group that can not hold a value.
    /// </summary>
    public byte[]? Value { get; set; }

    /// <summary>
    /// The type of data point this state is targeting.
    /// </summary>
    public DataPointType DataPointType { get; set; }

    [SetsRequiredMembers]
    public State(string address)
    {
        Address = address;
    }

    [SetsRequiredMembers]
    public State(string address, DataPointType dataPointType)
    {
        Address = address;
        DataPointType = dataPointType;
    }

    // Used for deserialization from JSON.
    public State()
    {
            
    }
}
