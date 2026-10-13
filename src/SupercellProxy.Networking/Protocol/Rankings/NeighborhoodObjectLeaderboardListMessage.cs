using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Defines the neighborhood object leaderboard list message contract.
/// </summary>
public sealed record NeighborhoodObjectLeaderboardListMessage : CodecMessage<NeighborhoodObjectLeaderboardListMessage>, ICodecProvider<NeighborhoodObjectLeaderboardListMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodObjectLeaderboardListMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.Value0,
            Codecs.VarInt,
            static message => message.Value1,
            Codecs.VarInt,
            static message => message.Value2,
            RankingEntry.ListCodec,
            static message => message.Entries,
            static (value0, value1, value2, entries) => new NeighborhoodObjectLeaderboardListMessage
            {
                Value0 = value0,
                Value1 = value1,
                Value2 = value2,
                Entries = entries,
            }
        ),
        trailingDataMessage: "The player-ranking page has trailing data."
    );

    /// <summary>
    /// Gets the Entries value.
    /// </summary>
    public RankingEntry[]? Entries { get; init; }
    /// <summary>
    /// Gets the Value0 value.
    /// </summary>
    public int Value0 { get; init; }

    /// <summary>
    /// Gets the Value1 value.
    /// </summary>
    public int Value1 { get; init; }

    /// <summary>
    /// Gets the Value2 value.
    /// </summary>
    public int Value2 { get; init; }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(NeighborhoodObjectLeaderboardListMessage);
    }
}
