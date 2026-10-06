using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Sessions.Anonymous;

/// <summary>The credentials needed to reopen an anonymous game account.</summary>
public sealed record AnonymousAccountCredentials([property: JsonPropertyName("accountId")] long AccountId, [property: JsonPropertyName("passToken")] string PassToken)
{
    /// <summary>Omits credentials from diagnostics.</summary>
    public override string ToString()
    {
        return nameof(AnonymousAccountCredentials);
    }
}
