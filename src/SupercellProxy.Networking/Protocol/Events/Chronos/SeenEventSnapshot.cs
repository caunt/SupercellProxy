using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Record that the player looked at one showing (impression) of a targeted event, so the same showing is not announced again.
/// </summary>
public sealed record SeenEventSnapshot
{
    /// <summary>
    /// Gets or sets how many times this showing was looked at (saved as <c>cnt</c>).
    /// </summary>
    /// <value>A positive view count; 1 by default.</value>
    [JsonPropertyName("cnt")]
    public int Count { get; init; } = 1;

    /// <summary>
    /// Gets or sets the Unix seconds when the event ends (saved as <c>et</c>).
    /// </summary>
    /// <value>The end time of the event the record names; 0 when unset.</value>
    [JsonPropertyName("et")]
    public int EndTime { get; init; }

    /// <summary>
    /// Gets or sets the targeted event that was looked at (saved as <c>id</c>).
    /// </summary>
    /// <value>The id of the shop live event; together with <see cref="ImpressionId"/> it identifies the record.</value>
    [JsonPropertyName("id")]
    public int EventId { get; init; }

    /// <summary>
    /// Gets or sets which showing of the event it was (saved as <c>imp</c>); the same event can be shown to the player several times.
    /// </summary>
    /// <value>The impression id of the showing, as the event definition gives it.</value>
    [JsonPropertyName("imp")]
    public int ImpressionId { get; init; }

    /// <summary>
    /// Gets or sets the kind of record (saved as <c>st</c>); records for targeted events use 1.
    /// </summary>
    /// <value>The record kind as saved; the meaning of values other than 1 is not yet identified.</value>
    [JsonPropertyName("st")]
    public int Kind { get; init; }
}
