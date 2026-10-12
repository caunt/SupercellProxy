namespace SupercellProxy.Networking.Transport;

/// <summary>Describes completed game-stream I/O, including message headers and encrypted payload bytes.</summary>
public sealed record ConnectionTrafficSnapshot(
    DateTimeOffset ConnectedAt,
    TimeSpan AveragingPeriod,
    long ReceivedBytes,
    long SentBytes,
    double ReceivedBytesPerSecond,
    double SentBytesPerSecond
)
{
    /// <summary>Gets the rolling period used for traffic rates.</summary>
    public static TimeSpan AveragingWindow { get; } = TimeSpan.FromMinutes(minutes: 5);
}
