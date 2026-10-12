using Nito.AsyncEx;

namespace SupercellProxy.Networking.Transport;

/// <summary>Tracks one socket's byte totals and recent traffic using a monotonic clock.</summary>
internal sealed class ConnectionTraffic(TimeProvider timeProvider)
{
    private readonly AsyncLock _gate = new();
    private readonly Queue<Sample> _samples = new();
    private readonly long _started = timeProvider.GetTimestamp();
    private readonly DateTimeOffset _connectedAt = timeProvider.GetUtcNow();
    private long _received;
    private long _recentReceived;
    private long _recentSent;
    private long _sent;

    internal ConnectionTrafficSnapshot Read()
    {
        using (IDisposable lease = _gate.Lock())
        {
            long now = timeProvider.GetTimestamp();
            RemoveExpired(now);
            double seconds = Math.Clamp(timeProvider.GetElapsedTime(_started, now).TotalSeconds, min: 0, ConnectionTrafficSnapshot.AveragingWindow.TotalSeconds);

            return new(
                _connectedAt,
                TimeSpan.FromSeconds(seconds),
                _received,
                _sent,
                seconds > 0 ? _recentReceived / seconds : 0,
                seconds > 0 ? _recentSent / seconds : 0
            );
        }
    }

    internal void Receive(int bytes)
    {
        Record(bytes, sent: 0);
    }

    internal void Send(long bytes)
    {
        Record(received: 0, bytes);
    }

    private void Record(long received, long sent)
    {
        if (received == 0 && sent == 0) return;

        using (IDisposable lease = _gate.Lock())
        {
            long now = timeProvider.GetTimestamp();
            RemoveExpired(now);
            _received += received;
            _sent += sent;
            _recentReceived += received;
            _recentSent += sent;
            _samples.Enqueue(new(now, received, sent));
        }
    }

    private void RemoveExpired(long now)
    {
        while (_samples.TryPeek(out Sample sample))
        {
            if (timeProvider.GetElapsedTime(sample.Timestamp, now) < ConnectionTrafficSnapshot.AveragingWindow) break;

            Sample removed = _samples.Dequeue();
            _recentReceived -= removed.Received;
            _recentSent -= removed.Sent;
        }
    }

    private readonly record struct Sample(long Timestamp, long Received, long Sent);
}
