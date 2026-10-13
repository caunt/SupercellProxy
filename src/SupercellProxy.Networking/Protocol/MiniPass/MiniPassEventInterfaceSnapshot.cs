using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved presentation settings of a Mini Pass event.</summary>
public sealed record MiniPassEventInterfaceSnapshot
{
    /// <summary>Gets the displayed pass title.</summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }
}
