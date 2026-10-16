using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Clientbound page of a player ranking.
/// </summary>
public sealed record PlayerRankingsMessage : CodecMessage<PlayerRankingsMessage>, ICodecProvider<PlayerRankingsMessage>
{
    /// <inheritdoc />
    public static ICodec<PlayerRankingsMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(RankingEntry.ListCodec, static message => message.Entries, static entries => new PlayerRankingsMessage { Entries = entries, }),
        trailingDataMessage: "The player-ranking response has trailing data."
    );

    /// <summary>
    /// The ranking's lines in rank order; null when the list is absent.
    /// </summary>
    public RankingEntry[]? Entries { get; init; }

    /// <summary>
    /// Returns only the type name, keeping player names out of logs.
    /// </summary>
    public override string ToString()
    {
        return nameof(PlayerRankingsMessage);
    }
}
