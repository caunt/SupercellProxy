using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Reports a personal-train pickup request's result for a visited Town.</summary>
/// <param name="PickingHomeId">Home id of the player whose personal train asked for the passengers, or <see langword="null"/> when the server omits it.</param>
/// <param name="VisitedHomeId">Home id of the visited Town the passengers were requested from, or <see langword="null"/> when the server omits it.</param>
/// <param name="Unknown0">Third field, a variable-length integer whose meaning is not yet identified.</param>
/// <param name="Status">Result code: 0 when the pickup was accepted, any other value when the server refused it. The passenger reservations themselves arrive in a later server command.</param>
public sealed record PickUpPassengerResponseMessage(LongId? PickingHomeId, LongId? VisitedHomeId, int Unknown0, int Status) : CodecMessage<PickUpPassengerResponseMessage>, ICodecProvider<PickUpPassengerResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<PickUpPassengerResponseMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalLongId,
        static message => message.PickingHomeId,
        Codecs.OptionalLongId,
        static message => message.VisitedHomeId,
        Codecs.VarInt,
        static message => message.Unknown0,
        Codecs.VarInt,
        static message => message.Status,
        static (pickingHome, visitedHome, unknown0, status) => new PickUpPassengerResponseMessage(pickingHome, visitedHome, unknown0, status));
}
