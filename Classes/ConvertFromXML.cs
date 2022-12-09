using IfKThenX.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace IfKThenX;

/// <summary>
/// Provides usefull methods to convert from XML to objects used in IfKThenX.
/// </summary>
public static class ConvertFromXML
{
    /// <summary>
    /// Converts GroupAddress entries from an XML-file generated with the ETS to a list of <see cref="IState"/> objects.
    /// </summary>
    /// <param name="filePath">The absolute path to the XML-file.</param>
    /// <returns>A <see cref="Task"/> representing the process of generating the list of <see cref="IState"/>.</returns>
    public static async Task<List<IState>> ToIStatesAsync(string filePath)
    {
        List<IState> parsedStates = new List<IState>();
        XmlReaderSettings settings = new XmlReaderSettings { Async = true };

        using (XmlReader xmlFile = XmlReader.Create(filePath, settings))
        {
            XDocument xdoc = await XDocument.LoadAsync(xmlFile, LoadOptions.None, CancellationToken.None);
            XNamespace ns = @"http://knx.org/xml/ga-export/01";
            
            //TODO Validate xml data!

            IEnumerable<XElement> statesFromXml = from s in xdoc.Descendants(ns + "GroupAddress")
                                                  select s;

            if (statesFromXml is not null)
            {
                foreach (var s in statesFromXml)
                {
                    string? currentGroupAddress = s?.Attribute("Address")?.Value;
                    string? currentGroupName = s?.Attribute("Name")?.Value;
                    string? currentGroupDPTs = s?.Attribute("DPTs")?.Value;
                    string? currentGroupValue = s?.Attribute("Value")?.Value;
                    DataPointType? currentDpt = ConvertFromFalcon.ToDPT(currentGroupDPTs);

                    if (currentGroupAddress is not null && currentDpt.HasValue)
                    {
                        IState currentState = new State(currentGroupAddress, currentDpt.Value);
                        
                        bool isParseable = int.TryParse(currentGroupValue, out _);
                        if (isParseable)
                        {
                            int parsedInt = int.Parse(currentGroupValue!);
                            currentState.Value = BitConverter.GetBytes(parsedInt);
                        }
                        
                        isParseable = bool.TryParse(currentGroupValue!, out _);
                        if (isParseable)
                        {
                            bool parsedBool = bool.Parse(currentGroupValue!);
                            currentState.Value = BitConverter.GetBytes(parsedBool);
                        }

                        parsedStates.Add(currentState);
                    }
                }
            }
        }
        return parsedStates;
    }

    /// <summary>
    /// Converts GroupAddress entries from an XML-file generated with the ETS to an <see cref="IBusState"/>.
    /// </summary>
    /// <param name="filePath">The absolute path to the XML-file.</param>
    /// <returns>A <see cref="Task"/> representing the process of generating the <see cref="IBusState"/>.</returns>
    public static async Task<IBusState> ToIBusStateAsync(string filePath)
    {
        List<IState> states = await ToIStatesAsync(filePath);
        var busState = new BusState();
        foreach (var state in states)
        {
            busState.AddOrUpdateState(state);
        }
        return busState;
    }
}
