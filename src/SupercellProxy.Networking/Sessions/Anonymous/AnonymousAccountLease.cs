namespace SupercellProxy.Networking.Sessions.Anonymous;

/// <summary>Exclusively owns an anonymous account until its connection is closed.</summary>
public abstract class AnonymousAccountLease : IAsyncDisposable
{
    /// <summary>Gets saved credentials, or null while creating a new account.</summary>
    public abstract AnonymousAccountCredentials? Credentials { get; }
    /// <summary>Returns the account to the pool after its transport has closed.</summary>
    public abstract ValueTask DisposeAsync();

    /// <summary>Updates public farm information without persisting connection state.</summary>
    public abstract void Observe(AnonymousAccountProfile profile);

    /// <summary>Removes credentials explicitly rejected by the authentication server.</summary>
    public abstract Task RejectAsync(CancellationToken cancellationToken);

    /// <summary>Persists the credentials acknowledged by the server before the connection is exposed.</summary>
    public abstract Task SaveAsync(AnonymousAccountCredentials credentials, CancellationToken cancellationToken);
}
