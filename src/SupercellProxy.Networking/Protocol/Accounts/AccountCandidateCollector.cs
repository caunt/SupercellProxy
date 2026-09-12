
namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>
/// Defines the Account Load Candidates contract.
/// </summary>
public sealed class AccountCandidateCollector
{

    /// <summary>
    /// Gets the Entries Response value.
    /// </summary>
    public AccountCandidatesMessage? EntriesResponse { get; private set; }
    /// <summary>
    /// Gets the Response value.
    /// </summary>
    public AccountLoadResponseMessage? Response { get; private set; }

    /// <summary>
    /// Gets the Response Entry Count value.
    /// </summary>
    public int? ResponseEntryCount { get; private set; }

    /// <summary>
    /// Provides the Apply value or operation.
    /// </summary>
    public void Apply(AccountCandidatesMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);
        ResponseEntryCount = response.EntryCount;
        EntriesResponse = response;
    }

    /// <summary>
    /// Provides the Apply value or operation.
    /// </summary>
    public void Apply(AccountLoadResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);
        Response = response;
    }
}
