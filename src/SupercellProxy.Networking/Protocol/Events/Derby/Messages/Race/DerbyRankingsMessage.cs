using SupercellProxy.Networking.Protocol.Events.Derby.Race;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

/// <summary>Returns the optional ordered derby neighborhood rankings.</summary>
public sealed record DerbyRankingsMessage(DerbyRankingEntry[]? Entries) : CodecMessage<DerbyRankingsMessage>, ICodecProvider<DerbyRankingsMessage>
{
    /// <inheritdoc />
    public static ICodec<DerbyRankingsMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.NullableArray(DerbyRankingEntry.Codec, CollectionCount.VarInt.WithStrictAbsence()),
            static message => message.Entries,
            static entries => new DerbyRankingsMessage(entries)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits neighborhood details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyRankingsMessage);
    }
}
