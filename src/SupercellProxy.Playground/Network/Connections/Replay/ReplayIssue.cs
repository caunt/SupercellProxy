namespace SupercellProxy.Playground.Network.Connections.Replay;

/// Describes one frame-level replay exception or mismatch.
internal sealed record ReplayIssue(
    string Direction,
    int Sequence,
    int? MessageId,
    int? MessageVersion,
    string ErrorType,
    string Error
);
