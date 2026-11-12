namespace SupercellProxy.Networking.Transport;

/// <summary>Describes completed game-stream I/O, including message headers and encrypted payload bytes.</summary>
/// <param name="ConnectedAt">UTC time the connection's traffic counter was created.</param>
/// <param name="AveragingPeriod">Time the rates are averaged over: the connection's age, capped at <see cref="AveragingWindow"/>.</param>
/// <param name="ReceivedBytes">Total bytes read from the socket since the connection opened, frame headers included.</param>
/// <param name="SentBytes">Total bytes written to the socket since the connection opened, frame headers included.</param>
/// <param name="ReceivedBytesPerSecond">Bytes received within the last <see cref="AveragingWindow"/> divided by <paramref name="AveragingPeriod"/> in seconds; 0 when that period is zero.</param>
/// <param name="SentBytesPerSecond">Bytes sent within the last <see cref="AveragingWindow"/> divided by <paramref name="AveragingPeriod"/> in seconds; 0 when that period is zero.</param>
public sealed record ConnectionTrafficSnapshot(DateTimeOffset ConnectedAt,
    TimeSpan AveragingPeriod,
    long ReceivedBytes,
    long SentBytes,
    double ReceivedBytesPerSecond,
    double SentBytesPerSecond)
{
    /// <summary>Gets the rolling period used for traffic rates.</summary>
    /// <value>Five minutes; traffic older than this drops out of the per-second rates but stays in the byte totals.</value>
    public static TimeSpan AveragingWindow { get; } = TimeSpan.FromMinutes(minutes: 5);
}
