using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Rewards;

/// <summary>Preserves the last presented values for a derby leaderboard entry.</summary>
public sealed record DerbyEntryState : ICodecProvider<DerbyEntryState>
{
    /// <inheritdoc />
    public static ICodec<DerbyEntryState> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
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
        }
    );

    /// <summary>Gets the presented bunny score.</summary>
    [JsonPropertyName("bunnyScore")]
    public int BunnyScore { get; init; }
    /// <summary>Gets the derby identifier when both words were saved.</summary>
    [JsonIgnore]
    public LongId? DerbyId => DerbyIdHigh is { } high && DerbyIdLow is { } low ? new(high, low) : null;
    /// <summary>Gets the optional high word of the derby identifier.</summary>
    [JsonPropertyName("didh")]
    public int? DerbyIdHigh { get; init; }
    /// <summary>Gets the optional low word of the derby identifier.</summary>
    [JsonPropertyName("didl")]
    public int? DerbyIdLow { get; init; }
    /// <summary>Gets the leaderboard entry identifier.</summary>
    [JsonIgnore]
    public LongId Id => new(IdHigh, IdLow);
    /// <summary>Gets the high word of the leaderboard entry identifier.</summary>
    [JsonPropertyName("idh")]
    public int IdHigh { get; init; }
    /// <summary>Gets the low word of the leaderboard entry identifier.</summary>
    [JsonPropertyName("idl")]
    public int IdLow { get; init; }
    /// <summary>Gets the presented leaderboard order.</summary>
    [JsonPropertyName("ord")]
    public int Order { get; init; }
    /// <summary>Gets the presented score.</summary>
    [JsonPropertyName("scr")]
    public int Score { get; init; }
}
