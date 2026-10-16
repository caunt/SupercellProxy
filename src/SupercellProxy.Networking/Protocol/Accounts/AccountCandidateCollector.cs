using SupercellProxy.Networking.Protocol.Accounts.Binding;


namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>
/// Keeps the latest copy of two clientbound replies a session may receive: the farm list of <see cref="AccountCandidatesMessage"/>
/// and the Google Play already-bound notice.
/// </summary>
public sealed class AccountCandidateCollector
{
    /// <summary>
    /// The most recent farm list received; null until one arrives.
    /// </summary>
    public AccountCandidatesMessage? EntriesResponse { get; private set; }
    /// <summary>
    /// The most recent notice that the Google Play account is already bound to another farm; null until one arrives.
    /// </summary>
    public GoogleServiceAccountAlreadyBoundMessage? Response { get; private set; }

    /// <summary>
    /// Number of farms in the most recent list, -1 when that list was sent as absent, and null before any list arrived.
    /// </summary>
    public int? ResponseEntryCount { get; private set; }

    /// <summary>
    /// Stores a newly received farm list, replacing the previous one.
    /// </summary>
    /// <param name="response">The received list.</param>
    public void Apply(AccountCandidatesMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);
        ResponseEntryCount = response.EntryCount;
        EntriesResponse = response;
    }

    /// <summary>
    /// Stores a newly received Google Play already-bound notice, replacing the previous one.
    /// </summary>
    /// <param name="response">The received notice.</param>
    public void Apply(GoogleServiceAccountAlreadyBoundMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);
        Response = response;
    }
}
