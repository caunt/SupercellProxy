using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Reports a personal-train pickup request's result for a visited Town.</summary>
public sealed record PickUpPassengerResponseMessage(LongId? PickingHomeId, LongId? VisitedHomeId, int Unknown0, int Status) : CodecMessage<PickUpPassengerResponseMessage>, ICodecProvider<PickUpPassengerResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<PickUpPassengerResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static message => message.PickingHomeId,
        Codecs.OptionalLongId,
        static message => message.VisitedHomeId,
        Codecs.VarInt,
        static message => message.Unknown0,
        Codecs.VarInt,
        static message => message.Status,
        static (pickingHome, visitedHome, unknown0, status) => new PickUpPassengerResponseMessage(pickingHome, visitedHome, unknown0, status)
    );
}
