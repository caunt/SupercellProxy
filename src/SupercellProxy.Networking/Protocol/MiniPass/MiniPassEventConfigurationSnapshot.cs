using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved event configuration of a Mini Pass.</summary>
public sealed record MiniPassEventConfigurationSnapshot
{
    /// <summary>Gets the presentation settings.</summary>
    [JsonPropertyName("UI")]
    public MiniPassEventInterfaceSnapshot? Interface { get; init; }

    /// <summary>Gets the Mini Pass data row name.</summary>
    [JsonPropertyName("passType")]
    public string? PassType { get; init; }
}
