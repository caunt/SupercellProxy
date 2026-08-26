namespace SupercellProxy.Keys;

internal sealed record KeysUpdateResult(
    string AppName,
    string? Version,
    KeysUpdateOutcome Outcome,
    string? Key,
    string Reason,
    bool IsWarning = false
);
