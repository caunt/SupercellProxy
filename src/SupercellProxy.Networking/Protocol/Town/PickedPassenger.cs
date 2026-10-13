using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>
/// Represents <c language="csharp">PickedPassenger</c>.
/// </summary>
/// <param name="Unknown0">The <c language="csharp">Unknown0</c> value.</param>
/// <param name="Unknown1">The <c language="csharp">Unknown1</c> value.</param>
/// <param name="PassengerGlobalId">The <c language="csharp">PassengerGlobalId</c> value.</param>
/// <param name="PickupHomeId">The <c language="csharp">PickupHomeId</c> value.</param>
/// <param name="UnknownId1">The <c language="csharp">UnknownId1</c> value.</param>
/// <param name="PassengerDocument">The <c language="csharp">PassengerDocument</c> value.</param>
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
