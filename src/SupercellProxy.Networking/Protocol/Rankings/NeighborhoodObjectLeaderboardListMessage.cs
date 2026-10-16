using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Clientbound leaderboard page for a neighbourhood object event.
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
    /// The board's lines in rank order; null when the list is absent.
    /// </summary>
    public RankingEntry[]? Entries { get; init; }
    /// <summary>
    /// First of three numbers sent before the lines; its meaning is not identified.
    /// </summary>
    public int Value0 { get; init; }

    /// <summary>
    /// Second of the three numbers; not identified.
    /// </summary>
    public int Value1 { get; init; }

    /// <summary>
    /// Third of the three numbers; not identified.
    /// </summary>
    public int Value2 { get; init; }

    /// <summary>
    /// Returns only the type name, keeping player names out of logs.
    /// </summary>
    public override string ToString()
    {
        return nameof(NeighborhoodObjectLeaderboardListMessage);
    }
}
