namespace SupercellProxy.Playground.Network.Connections.Replay;

/// Summarizes every capture discovered beneath one replay root.
internal sealed record ReplayBatch(Memory<CaptureReplay> Captures)
{
    /// Gets the total number of replayed captures.
    public int CaptureCount => Captures.Length;

    /// Gets the number of captures that matched exactly.
    public int ExactCount
    {
        get
        {
            var count = 0;

            foreach (var capture in Captures.Span)
            {
                if (capture.Exact)
                    count++;
            }

            return count;
        }
    }

    /// Gets the number of captures that failed or diverged.
    public int FailureCount => Captures.Length - ExactCount;
}
