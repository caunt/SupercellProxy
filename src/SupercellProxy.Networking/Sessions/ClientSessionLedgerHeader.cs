namespace SupercellProxy.Networking.Sessions;

internal sealed record ClientSessionLedgerHeader
{
    public int? Version { get; init; }
}
