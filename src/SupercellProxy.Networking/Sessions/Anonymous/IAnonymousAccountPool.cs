namespace SupercellProxy.Networking.Sessions.Anonymous;

/// <summary>Supplies exclusive anonymous accounts without coupling transport to a persistence implementation.</summary>
public interface IAnonymousAccountPool
{
    /// <summary>Leases the requested account, or an idle account/new-account reservation when no id is specified.</summary>
    Task<AnonymousAccountLease> AcquireAsync(long? accountId, CancellationToken cancellationToken);
}
