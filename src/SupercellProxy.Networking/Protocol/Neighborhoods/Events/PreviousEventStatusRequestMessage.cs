using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Events;

/// <summary>Requests the final standings of the previous neighborhood event, answered by PreviousEventStatusResponseMessage.</summary>
public sealed record PreviousEventStatusRequestMessage : CodecMessage<PreviousEventStatusRequestMessage>, ICodecProvider<PreviousEventStatusRequestMessage>
{
    /// <inheritdoc />
    public static ICodec<PreviousEventStatusRequestMessage> Codec { get; } = Codecs.Unit(static () => new PreviousEventStatusRequestMessage());
}
