using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>App data on a decrypt.day app page: the app and its known releases.</summary>
internal sealed record DecryptDayApplicationPayload
{
    /// <summary>The app the page is about (saved as <c>app</c>); null on page parts that describe something else.</summary>
    [JsonPropertyName("app")]
    public DecryptDayApplicationMetadata? Application { get; init; }

    /// <summary>Releases of the app that decrypt.day knows about.</summary>
    [JsonPropertyName("versions")]
    public DecryptDayVersionMetadata?[] Versions { get; init; } = [];
}
