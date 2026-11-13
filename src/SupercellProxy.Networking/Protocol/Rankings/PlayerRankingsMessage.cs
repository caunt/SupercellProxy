using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Clientbound page of a player ranking.
/// </summary>
public sealed record PlayerRankingsMessage : CodecMessage<PlayerRankingsMessage>, ICodecProvider<PlayerRankingsMessage>
{
    /// <inheritdoc />
    public static ICodec<PlayerRankingsMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(RankingEntry.ListCodec,
            static message => message.Entries,
            static entries => new PlayerRankingsMessage
            {
                Entries = entries,
            }),
        trailingDataMessage: "The player-ranking response has trailing data.");

    /// <summary>
    /// Gets or sets the ranking's lines in rank order; <see langword="null"/> when the list is absent.
    /// </summary>
    /// <value>The whole payload: entries with rank, score, account, farm name, level and farm; <see langword="null"/> when native wrote a count of -1, and empty when it wrote 0.</value>
    public ImmutableArray<RankingEntry>? Entries { get; init; }

    /// <summary>
    /// Returns only the type name, keeping player names out of logs.
    /// </summary>
    /// <returns>The constant text <c>PlayerRankingsMessage</c>, whatever the page holds.</returns>
    public override string ToString()
    {
        return nameof(PlayerRankingsMessage);
    }
}
