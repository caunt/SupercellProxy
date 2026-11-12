namespace SupercellProxy.Networking.Sessions.Anonymous;

/// <summary>Exclusively owns an anonymous account until its connection is closed.</summary>
public abstract class AnonymousAccountLease : IAsyncDisposable
{
    /// <summary>Gets saved credentials, or <see langword="null"/> while creating a new account.</summary>
    /// <value>The account id and pass token the next login should send; <see langword="null"/> for a new-account reservation until <see cref="SaveAsync"/> records the account the server created.</value>
    public abstract AnonymousAccountCredentials? Credentials { get; }
    /// <summary>Returns the account to the pool after its transport has closed.</summary>
    public abstract ValueTask DisposeAsync();

    /// <summary>Updates public farm information without persisting connection state.</summary>
    /// <param name="profile">Farm name, level, experience and balances read from the account's own avatar in the latest home data; ignored while the lease has no credentials or is disposed.</param>
    public abstract void Observe(AnonymousAccountProfile profile);

    /// <summary>Removes credentials explicitly rejected by the authentication server.</summary>
    /// <param name="cancellationToken">Stops waiting for the pool's lock or for the saved account list to be rewritten.</param>
    public abstract Task RejectAsync(CancellationToken cancellationToken);

    /// <summary>Persists the credentials acknowledged by the server before the connection is exposed.</summary>
    /// <param name="credentials">Account id and pass token from the accepted login; for a lease of a saved account the id must match it, and for a new-account reservation it becomes the leased account.</param>
    /// <param name="cancellationToken">Stops waiting for the pool's lock or the save; callers pass a short dedicated timeout so a replacement pass token is not lost to the connection's own cancellation.</param>
    public abstract Task SaveAsync(AnonymousAccountCredentials credentials, CancellationToken cancellationToken);
}
