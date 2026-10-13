using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Defines the Player Rankings Message contract.
/// </summary>
public sealed record PlayerRankingsMessage : CodecMessage<PlayerRankingsMessage>, ICodecProvider<PlayerRankingsMessage>
{
    /// <inheritdoc />
    public static ICodec<PlayerRankingsMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(RankingEntry.ListCodec, static message => message.Entries, static (entries) => new PlayerRankingsMessage { Entries = entries, }),
        trailingDataMessage: "The player-ranking response has trailing data."
    );

    /// <summary>
    /// Gets the Entries value.
    /// </summary>
    public RankingEntry[]? Entries { get; init; }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(PlayerRankingsMessage);
    }
}
