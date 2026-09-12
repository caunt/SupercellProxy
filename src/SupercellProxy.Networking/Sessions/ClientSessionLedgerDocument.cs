namespace SupercellProxy.Networking.Sessions;

/// <summary>Defines the concrete JSON document stored by <see cref="ClientSessionLedger"/>.</summary>
internal sealed record ClientSessionLedgerDocument
{
    /// <summary>Gets the retained account sessions.</summary>
    public ClientSession?[]? Sessions { get; init; } = [];
}
