namespace SupercellProxy.Networking.Protocol.Timing;

/// <summary>Identifies the representation of an optional game-object timer.</summary>
public enum TimerValueKind
{
    /// <summary>The timer field was absent.</summary>
    Missing,
    /// <summary>The timer field was explicitly null.</summary>
    Null,
    /// <summary>The timer is an integer counter.</summary>
    Counter,
    /// <summary>The timer contains a start time and remaining ticks.</summary>
    Countdown,
}
