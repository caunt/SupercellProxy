using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>
/// A passenger the player's personal train picked up in a neighbour's town; it gets off the train when the train reaches the
/// player's own town.
/// </summary>
/// <param name="Unknown0">First number of the entry; its meaning is not identified.</param>
/// <param name="Unknown1">Second number of the entry; its meaning is not identified.</param>
/// <param name="PassengerGlobalId">Global id of the passenger's table entry; entries outside the passenger table are skipped on arrival.</param>
/// <param name="PickupHomeId">Home whose personal train picked the passenger up; the passenger keeps it once it arrives.</param>
/// <param name="UnknownId1">Second home or account id of the entry; its meaning is not identified.</param>
/// <param name="PassengerDocument">The passenger's saved game object state as JSON, restored when it arrives; null or empty for a fresh passenger.</param>
public sealed record PickedPassenger(int Unknown0, int Unknown1, int PassengerGlobalId, LongId PickupHomeId, LongId UnknownId1, string? PassengerDocument) : ICodecProvider<PickedPassenger>
{
    /// <inheritdoc />
    public static ICodec<PickedPassenger> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Unknown0,
        Codecs.VarInt,
        static value => value.Unknown1,
        Codecs.VarInt,
        static value => value.PassengerGlobalId,
        Codecs.LongId,
        static value => value.PickupHomeId,
        Codecs.LongId,
        static value => value.UnknownId1,
        Codecs.OptionalString,
        static value => value.PassengerDocument,
        static (unknown0, unknown1, passengerGlobalId, pickupHomeId, unknownId1, passengerDocument) => new PickedPassenger(unknown0, unknown1, passengerGlobalId, pickupHomeId, unknownId1, passengerDocument)
    );
}
