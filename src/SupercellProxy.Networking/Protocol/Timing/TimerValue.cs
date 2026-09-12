using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Timing;

/// <summary>A game-object timer encoded as either a counter or a countdown snapshot.</summary>
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(TimerValueConverter))]
public readonly record struct TimerValue
{
    internal TimerValue(TimerValueKind kind, int counter, TimerSnapshot countdown)
    {
        Kind = kind;
        Counter = counter;
        Countdown = countdown;
    }

    /// <summary>Gets the counter value.</summary>
    public int Counter { get; }

    /// <summary>Gets the countdown state.</summary>
    public TimerSnapshot Countdown { get; }

    /// <summary>Gets whether a timer was supplied.</summary>
    public bool HasValue => Kind is TimerValueKind.Counter or TimerValueKind.Countdown;

    /// <summary>Gets the encoded timer variant.</summary>
    public TimerValueKind Kind { get; }

    /// <summary>Creates a countdown timer.</summary>
    public static TimerValue FromCountdown(TimerSnapshot countdown)
    {
        return new(TimerValueKind.Countdown, counter: 0, countdown);
    }

    /// <summary>Creates an integer counter.</summary>
    public static TimerValue FromCounter(int counter)
    {
        return new(TimerValueKind.Counter, counter, countdown: default);
    }

    /// <summary>Returns the countdown state and rejects an integer counter representation.</summary>
    public TimerSnapshot? AsCountdown()
    {
        return Kind is TimerValueKind.Counter
        ? throw new InvalidDataException(message: "The timer contains a counter instead of a countdown.")
        : Kind is TimerValueKind.Countdown ? Countdown : null;
    }

    /// <summary>Returns the counter value and rejects a countdown representation.</summary>
    public int AsCounter()
    {
        return Kind is TimerValueKind.Countdown
        ? throw new InvalidDataException(message: "The timer contains a countdown instead of a counter.")
        : Counter;
    }
}
