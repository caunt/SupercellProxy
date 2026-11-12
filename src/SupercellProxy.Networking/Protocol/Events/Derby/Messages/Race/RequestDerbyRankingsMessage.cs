using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

/// <summary>Requests derby rankings, optionally selecting a neighborhood.</summary>
/// <param name="NeighborhoodId">The neighborhood to select in the rankings, or <see langword="null"/> to request them without one; the server answers with <see cref="DerbyRankingsMessage"/>.</param>
public sealed record RequestDerbyRankingsMessage(LongId? NeighborhoodId) : CodecMessage<RequestDerbyRankingsMessage>, ICodecProvider<RequestDerbyRankingsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDerbyRankingsMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.OptionalLongId,
            static message => message.NeighborhoodId,
            static neighborhoodId => new RequestDerbyRankingsMessage(neighborhoodId)),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Omits the neighborhood identifier from diagnostic text.</summary>
    /// <returns>The type name <c>RequestDerbyRankingsMessage</c> alone, so the selected neighborhood identifier never reaches logs.</returns>
    public override string ToString()
    {
        return nameof(RequestDerbyRankingsMessage);
    }
}
