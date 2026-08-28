namespace SupercellProxy.Playground.Network.Connections.Replay;

/// Describes one offline capture replay and its first failure, if any.
internal sealed record CaptureReplay(
    string Location,
    bool Exact,
    int ClientboundFrames,
    int ServerboundFrames,
    int GeneratedResponses,
    Memory<ReplayIssue> Issues
);
