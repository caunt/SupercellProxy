using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Rewards;

/// <summary>Preserves the last presented values for a derby leaderboard entry.</summary>
public sealed record DerbyEntryState : ICodecProvider<DerbyEntryState>
{
    /// <inheritdoc />
    public static ICodec<DerbyEntryState> Codec { get; } = Codecs.Composite(Codecs.LongId,
        static state => state.Id,
        Codecs.VarInt,
        static state => state.Score,
        Codecs.VarInt,
        static state => state.Order,
        Codecs.VarInt,
        static state => state.BunnyScore,
        Codecs.OptionalLongId,
        static state => state.DerbyId,
        static (id, score, order, bunnyScore, derbyId) => new DerbyEntryState
        {
            IdHigh = id.HighInt32,
            IdLow = id.LowInt32,
            Score = score,
            Order = order,
            BunnyScore = bunnyScore,
            DerbyIdHigh = derbyId?.HighInt32,
            DerbyIdLow = derbyId?.LowInt32,
        });

    /// <summary>Gets or sets the presented bunny score.</summary>
    /// <value>The bunny score last shown for the entry, saved as <c>bunnyScore</c> and written after <see cref="Order"/> on the wire.</value>
    [JsonPropertyName("bunnyScore")]
    public int BunnyScore { get; init; }
    /// <summary>Gets the derby identifier when both words were saved.</summary>
    /// <value>The identifier built from <see cref="DerbyIdHigh"/> and <see cref="DerbyIdLow"/>, or <see langword="null"/> when either word is missing; it is computed on read and never saved to JSON.</value>
    [JsonIgnore]
    public LongId? DerbyId => DerbyIdHigh is { } high && DerbyIdLow is { } low ? new(high, low) : null;
    /// <summary>Gets or sets the optional high word of the derby identifier.</summary>
    /// <value>The upper 32 bits of the derby identifier, saved as <c>didh</c>, or <see langword="null"/> when the entry has no derby recorded.</value>
    [JsonPropertyName("didh")]
    public int? DerbyIdHigh { get; init; }
    /// <summary>Gets or sets the optional low word of the derby identifier.</summary>
    /// <value>The lower 32 bits of the derby identifier, saved as <c>didl</c>, or <see langword="null"/> when the entry has no derby recorded.</value>
    [JsonPropertyName("didl")]
    public int? DerbyIdLow { get; init; }
    /// <summary>Gets the leaderboard entry identifier.</summary>
    /// <value>The identifier built from <see cref="IdHigh"/> and <see cref="IdLow"/>; it is what the wire form carries first and is never saved to JSON as one value.</value>
    [JsonIgnore]
    public LongId Id => new(IdHigh, IdLow);
    /// <summary>Gets or sets the high word of the leaderboard entry identifier.</summary>
    /// <value>The upper 32 bits of <see cref="Id"/>, saved as <c>idh</c>.</value>
    [JsonPropertyName("idh")]
    public int IdHigh { get; init; }
    /// <summary>Gets or sets the low word of the leaderboard entry identifier.</summary>
    /// <value>The lower 32 bits of <see cref="Id"/>, saved as <c>idl</c>.</value>
    [JsonPropertyName("idl")]
    public int IdLow { get; init; }
    /// <summary>Gets or sets the presented leaderboard order.</summary>
    /// <value>The position the entry had on the leaderboard when it was last shown, saved as <c>ord</c>.</value>
    [JsonPropertyName("ord")]
    public int Order { get; init; }
    /// <summary>Gets or sets the presented score.</summary>
    /// <value>The score last shown for the entry, saved as <c>scr</c> and written right after <see cref="Id"/> on the wire.</value>
    [JsonPropertyName("scr")]
    public int Score { get; init; }
}
