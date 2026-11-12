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

    /// <summary>Gets the plain number stored when the timer is the counter form; 0 for the countdown form.</summary>
    /// <value>The integer the save held in place of a countdown object when <see cref="Kind"/> is <see cref="TimerValueKind.Counter"/>; 0 for every other kind.</value>
    public int Counter { get; }

    /// <summary>Gets the countdown state.</summary>
    /// <value>The start time and remaining ticks when <see cref="Kind"/> is <see cref="TimerValueKind.Countdown"/>; for every other kind, the default snapshot with both fields 0.</value>
    public TimerSnapshot Countdown { get; }

    /// <summary>Gets whether a timer was supplied.</summary>
    /// <value><see langword="true"/> if the timer is a counter or a countdown; otherwise, <see langword="false"/>.</value>
    /// <remarks>A JSON field that was absent or <see langword="null"/> gives <see langword="false"/>.</remarks>
    public bool HasValue => Kind is TimerValueKind.Counter or TimerValueKind.Countdown;

    /// <summary>Gets the encoded timer variant.</summary>
    /// <value><see cref="TimerValueKind.Counter"/> for a bare JSON number, <see cref="TimerValueKind.Countdown"/> for a timer object, <see cref="TimerValueKind.Null"/> for an explicit JSON <see langword="null"/>, and <see cref="TimerValueKind.Missing"/>, the default, when the field was not in the save.</value>
    public TimerValueKind Kind { get; }

    /// <summary>Creates a countdown timer.</summary>
    /// <param name="countdown">The start time and remaining ticks to store; it is kept as given, including a countdown that has already run out.</param>
    /// <returns>A timer of kind <see cref="TimerValueKind.Countdown"/> that serializes back to a timer object and whose <see cref="Counter"/> is 0.</returns>
    public static TimerValue From(TimerSnapshot countdown)
    {
        return new(TimerValueKind.Countdown, counter: 0, countdown);
    }

    /// <summary>Creates an integer counter.</summary>
    /// <param name="counter">The plain number to store; any value is accepted and serialized back as a bare JSON number.</param>
    /// <returns>A timer of kind <see cref="TimerValueKind.Counter"/> whose <see cref="Countdown"/> is the default snapshot.</returns>
    public static TimerValue From(int counter)
    {
        return new(TimerValueKind.Counter, counter, countdown: default);
    }

    /// <summary>Returns the countdown state and rejects an integer counter representation.</summary>
    /// <returns><see cref="Countdown"/> when <see cref="Kind"/> is <see cref="TimerValueKind.Countdown"/>, or <see langword="null"/> when the timer is missing or JSON <see langword="null"/>.</returns>
    /// <exception cref="InvalidDataException"><see cref="Kind"/> is <see cref="TimerValueKind.Counter"/>: the save holds a plain number where a countdown object was expected.</exception>
    public TimerSnapshot? AsCountdown()
    {
        return Kind is TimerValueKind.Counter
        ? throw new InvalidDataException(message: "The timer contains a counter instead of a countdown.")
        : Kind is TimerValueKind.Countdown ? Countdown : null;
    }

    /// <summary>Returns the counter value and rejects a countdown representation.</summary>
    /// <returns><see cref="Counter"/> when <see cref="Kind"/> is <see cref="TimerValueKind.Counter"/>, and 0 when the timer is missing or JSON <see langword="null"/>.</returns>
    /// <exception cref="InvalidDataException"><see cref="Kind"/> is <see cref="TimerValueKind.Countdown"/>: the save holds a timer object where a plain number was expected.</exception>
    public int AsCounter()
    {
        return Kind is TimerValueKind.Countdown
        ? throw new InvalidDataException(message: "The timer contains a countdown instead of a counter.")
        : Counter;
    }
}
