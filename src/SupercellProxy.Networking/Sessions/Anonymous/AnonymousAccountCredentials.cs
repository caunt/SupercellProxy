using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Sessions.Anonymous;

/// <summary>The credentials needed to reopen an anonymous game account.</summary>
/// <param name="AccountId">The game account id as one 64-bit number (high word shifted left by 32 plus the low word), saved as <c>accountId</c>; it is sent as the login's account id.</param>
/// <param name="PassToken">Secret pass token the server issued for the account, saved as <c>passToken</c>; the login sends it in place of a session token, and the server may hand back a replacement that must then be saved.</param>
public sealed record AnonymousAccountCredentials([property: JsonPropertyName("accountId")] long AccountId, [property: JsonPropertyName("passToken")] string PassToken)
{
    /// <summary>Omits credentials from diagnostics.</summary>
    /// <returns>The type name <c>AnonymousAccountCredentials</c> only, never the account id or pass token, so the record can be logged safely.</returns>
    public override string ToString()
    {
        return nameof(AnonymousAccountCredentials);
    }
}
