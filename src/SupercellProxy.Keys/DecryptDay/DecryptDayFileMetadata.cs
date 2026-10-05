using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayFileMetadata response contract.</summary>
internal sealed record DecryptDayFileMetadata
{
    /// <summary>Gets the Id value.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>Gets the LoginRequired value.</summary>
    [JsonPropertyName("login_required")]
    public bool? LoginRequired { get; init; }

    /// <summary>Gets the Premium value.</summary>
    [JsonPropertyName("premium")]
    public bool? Premium { get; init; }
}
