using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// A town passenger reserved for pickup, with the home picking it up once a helper claims it.
/// </summary>
public sealed record PassengerReservation(LongId? PickerHomeId, int HelpId, bool Unknown1) : ICodecProvider<PassengerReservation>
{
    /// <inheritdoc />
    public static ICodec<PassengerReservation> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static value => value.PickerHomeId,
        Codecs.VarInt,
        static value => value.HelpId,
        Codecs.Boolean,
        static value => value.Unknown1,
        static (pickerHomeId, helpId, unknown1) => new PassengerReservation(pickerHomeId, helpId, unknown1)
    );
}
