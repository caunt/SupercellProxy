using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved presentation settings of a Mini Pass event.</summary>
public sealed record MiniPassEventInterfaceSnapshot
{
    /// <summary>Gets or sets the displayed pass title.</summary>
    /// <value>Saved as <c>title</c>; the text shown as the pass name, or <see langword="null"/> when the event gave none.</value>
    [JsonPropertyName("title")]
    public string? Title { get; init; }
}
