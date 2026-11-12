namespace SupercellProxy.Networking.Sessions.Anonymous;

/// <summary>Supplies exclusive anonymous accounts without coupling transport to a persistence implementation.</summary>
public interface IAnonymousAccountPool
{
    /// <summary>Leases the requested account, or an idle account/new-account reservation when no id is specified.</summary>
    /// <param name="accountId">Id of the saved account to lease, waiting while another connection holds it; <see langword="null"/> takes any idle saved account, or a reservation for a new one when none is idle.</param>
    /// <param name="cancellationToken">Stops waiting for the account to become free.</param>
    /// <returns>A lease that keeps the account exclusive until it is disposed; its <see cref="AnonymousAccountLease.Credentials"/> are <see langword="null"/> for a new-account reservation.</returns>
    Task<AnonymousAccountLease> AcquireAsync(long? accountId, CancellationToken cancellationToken);
}
