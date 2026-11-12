using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Clientbound leaderboard page for a neighbourhood object event.
/// </summary>
public sealed record NeighborhoodObjectLeaderboardListMessage : CodecMessage<NeighborhoodObjectLeaderboardListMessage>, ICodecProvider<NeighborhoodObjectLeaderboardListMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodObjectLeaderboardListMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
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
            }),
        trailingDataMessage: "The player-ranking page has trailing data.");

    /// <summary>
    /// Gets or sets the board's lines in rank order; <see langword="null"/> when the list is absent.
    /// </summary>
    /// <value>The entries that follow the three leading numbers, each with rank, score, account, farm name, level and farm; <see langword="null"/> when native wrote a count of -1, and empty when it wrote 0.</value>
    public RankingEntry[]? Entries { get; init; }
    /// <summary>
    /// Gets or sets the first of three numbers sent before the lines; its meaning is not identified.
    /// </summary>
    /// <value>The var-int at the very start of the payload; what it counts or identifies is not yet known.</value>
    public int Value0 { get; init; }

    /// <summary>
    /// Gets or sets the second of the three numbers; not identified.
    /// </summary>
    /// <value>The var-int that follows <see cref="Value0"/>; its meaning is not yet known.</value>
    public int Value1 { get; init; }

    /// <summary>
    /// Gets or sets the third of the three numbers; not identified.
    /// </summary>
    /// <value>The var-int that follows <see cref="Value1"/> and comes right before the entry list; its meaning is not yet known.</value>
    public int Value2 { get; init; }

    /// <summary>
    /// Returns only the type name, keeping player names out of logs.
    /// </summary>
    /// <returns>The constant text <c>NeighborhoodObjectLeaderboardListMessage</c>, whatever the page holds.</returns>
    public override string ToString()
    {
        return nameof(NeighborhoodObjectLeaderboardListMessage);
    }
}
