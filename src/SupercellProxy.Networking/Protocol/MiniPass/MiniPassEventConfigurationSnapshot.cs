using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved event configuration of a Mini Pass.</summary>
public sealed record MiniPassEventConfigurationSnapshot
{
    /// <summary>Gets or sets the presentation settings the live event gave the pass.</summary>
    /// <value>Saved as <c>UI</c>; holds the displayed title, or is <see langword="null"/> when the event supplied none.</value>
    [JsonPropertyName("UI")]
    public MiniPassEventInterfaceSnapshot? Interface { get; init; }

    /// <summary>Gets or sets the Mini Pass data row name.</summary>
    /// <value>Saved as <c>passType</c>; the name of the Mini Pass table row the event selects, or <see langword="null"/> when absent.</value>
    [JsonPropertyName("passType")]
    public string? PassType { get; init; }
}
