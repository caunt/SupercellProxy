using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayApplicationPayload response contract.</summary>
internal sealed record DecryptDayApplicationPayload
{
    /// <summary>Gets the Application value.</summary>
    [JsonPropertyName("app")]
    public DecryptDayApplicationMetadata? Application { get; init; }

    /// <summary>Gets the Versions value.</summary>
    [JsonPropertyName("versions")]
    public DecryptDayVersionMetadata?[] Versions { get; init; } = [];

}
