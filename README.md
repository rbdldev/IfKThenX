<p align="center">
    <img src="Logo.svg" width="600" max-width="90%" alt="StatiC#" />
</p>

<p align="center">
    <a href="https://docs.microsoft.com/en-us/dotnet/csharp/">
        <img src="https://img.shields.io/badge/C%23-11.0-blue?style=flat" alt="C# 11.0" />
    </a>
    <a href="https://dotnet.microsoft.com">
        <img src="https://img.shields.io/badge/.NET-7.0-blueviolet?style=flat" />
    </a>
    <img src="https://img.shields.io/badge/Platforms-Win+Mac+Linux-green?style=flat" />
    <img src="https://img.shields.io/static/v1?label=Version&message=0.1.0-alpha1&color=brightgreen" />
</p>

Welcome to **IfKThenX**, a logic layer that extends your KNX system with multi-dependent actions. It enables to define states of a bus, which are then monitored and handled with defined reactions.

---

The idea of **IfKThenX** is to respond to certain states within a KNX bus system by sending predefined telegrams.  

Let's give a very easy example: every time the light in the garage is turned on, you also want to turn on the lights in the courtyard entrance and turn off the lights in the living room. Then the state of turned-on lights in the garage is your condition *k*. If a condition is matched within the connected bus, the system will write the defined telegrams to the bus, in this case, to turn on the lights in the courtyard and off in the living room.  

A *k* can consist of as many sub-conditions as you want. Another example is that this *k* has the condition that the light in the hallway is on, and the front door is opened. The corresponding *x* could be to turn on the lights in the courtyard entrance.

## Quick start

You can operate with IfKThenX using the `IBusManager` service with the concrete type `BusManager`. Initiate a new `IBusManager` and inject your `ConnectionParameters`. A required parameter is the IP address of your KNX-IP interface as a string. That's all you need to get IfKThenX running. Now you can connect to your KNX bus system.

```CSharp
IConnectionParametes connection = new ConnectionParameters("192.0.2.1");
IBusManager bus = new BusManager(connection);
await bus.ConnectAsync();
```

IfKThenX will automatically start monitoring the bus activity.  
Finally, add one or more conditions. A condition is built of two parts, the **k** and the **x**. Every **k** can contain multiple **x**s. Under the hood, both are different states of the bus. If the state (can be a subgroup of the hole bus) within **k** is matched, the states in all corresponding **x**s are activated by sending the needed telegrams.
