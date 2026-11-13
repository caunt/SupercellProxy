using SupercellProxy.Networking.Protocol.Accounts.Binding;


namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>
/// Keeps the latest copy of two clientbound replies a session may receive: the farm list of <see cref="AccountCandidatesMessage"/>
/// and the Google Play already-bound notice.
/// </summary>
public sealed class AccountCandidateCollector
{
    /// <summary>
    /// Gets the most recent farm list received; <see langword="null"/> until one arrives.
    /// </summary>
    /// <value>The <see cref="AccountCandidatesMessage"/> passed to the last <see cref="Apply(AccountCandidatesMessage)"/> call, replaced by each newer list.</value>
    public AccountCandidatesMessage? EntriesResponse { get; private set; }

    /// <summary>
    /// Gets the most recent notice that the Google Play account is already bound to another farm; <see langword="null"/> until one arrives.
    /// </summary>
    /// <value>The <see cref="GoogleServiceAccountAlreadyBoundMessage"/> passed to the last <see cref="Apply(GoogleServiceAccountAlreadyBoundMessage)"/> call, replaced by each newer notice.</value>
    public GoogleServiceAccountAlreadyBoundMessage? Response { get; private set; }

    /// <summary>
    /// Gets the number of farms in the most recent list, -1 when that list was sent as absent, and <see langword="null"/> before any list arrived.
    /// </summary>
    /// <value>The <see cref="AccountCandidatesMessage.EntryCount"/> of <see cref="EntriesResponse"/>, captured when that list was applied; 0 means the server sent an empty list.</value>
    public int? ResponseEntryCount { get; private set; }

    /// <summary>
    /// Stores a newly received farm list, replacing the previous one.
    /// </summary>
    /// <param name="response">The received list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="response"/> is <see langword="null"/>.</exception>
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
    /// <exception cref="ArgumentNullException"><paramref name="response"/> is <see langword="null"/>; the stored notice is left as it was.</exception>
    public void Apply(GoogleServiceAccountAlreadyBoundMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);

        Response = response;
    }
}
