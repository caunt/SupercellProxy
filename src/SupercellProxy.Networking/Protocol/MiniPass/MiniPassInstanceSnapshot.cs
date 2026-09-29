using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved identity and progress for the current Mini Pass.</summary>
public sealed record MiniPassInstanceSnapshot
{
    /// <summary>Gets the Mini Pass data row.</summary>
    [JsonPropertyName("DataId")]
    public int DataGlobalIdentifier { get; init; }

    /// <summary>Gets the Mini Pass event identifier.</summary>
    [JsonPropertyName("EventId")]
    public int EventIdentifier { get; init; }

    /// <summary>Gets accumulated pass points.</summary>
    public int Points { get; init; }

    /// <summary>Gets the last points amount acknowledged by the player.</summary>
    public int SeenPoints { get; init; }
}
