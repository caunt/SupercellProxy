using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>A passenger at a town service building, as the server reports it after service help.</summary>
/// <param name="Unknown0">A value native's update (0x100496e10) does not read.</param>
/// <param name="PassengerObjectId">Global id of the passenger's game object; native removes a passenger with this id before adding the new one.</param>
/// <param name="Unknown2">A value native's update does not read.</param>
/// <param name="HelpId">Id of the passenger's help request; 0 when none was made.</param>
/// <param name="HelpKind">Who the help request asked: 1 town visitors, 2 the neighbourhood, 3 Farm Pass helpers.</param>
/// <param name="Unknown5">A value native copies onto the new passenger object (offset 0xfc); its meaning is not identified.</param>
/// <param name="PassengerGlobalId">Global id of the passenger's table entry.</param>
/// <param name="Services">Services the passenger wants, with what each one needs.</param>
/// <param name="Rewards">What the passenger pays for its services.</param>
/// <param name="BonusRewards">Extra reward the passenger gives once all its services are done.</param>
/// <param name="HelperHomeId">Home of the player who helped the service; null when nobody has.</param>
public sealed record TownServicePassenger(
    int Unknown0,
    int PassengerObjectId,
    int Unknown2,
    int HelpId,
    int HelpKind,
    int Unknown5,
    int PassengerGlobalId,
    ReadOnlyMemory<TownServiceRequest> Services,
    ReadOnlyMemory<TownServiceGood> Rewards,
    ReadOnlyMemory<TownServiceGood> BonusRewards,
    LongId? HelperHomeId
) : ICodecProvider<TownServicePassenger>
{
    /// <inheritdoc />
    public static ICodec<TownServicePassenger> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Unknown0,
        Codecs.VarInt,
        static value => value.PassengerObjectId,
        Codecs.VarInt,
        static value => value.Unknown2,
        Codecs.VarInt,
        static value => value.HelpId,
        Codecs.VarInt,
        static value => value.HelpKind,
        Codecs.VarInt,
        static value => value.Unknown5,
        Codecs.VarInt,
        static value => value.PassengerGlobalId,
        Codecs.Memory(TownServiceRequest.Codec),
        static value => value.Services,
        Codecs.Memory(TownServiceGood.Codec),
        static value => value.Rewards,
        Codecs.Memory(TownServiceGood.Codec),
        static value => value.BonusRewards,
        Codecs.OptionalLongId,
        static value => value.HelperHomeId,
        static (
            unknown0,
            passengerObjectId,
            unknown2,
            helpId,
            helpKind,
            unknown5,
            passengerGlobalId,
            services,
            rewards,
            bonusRewards,
            helperHomeId
        ) => new TownServicePassenger(
            unknown0,
            passengerObjectId,
            unknown2,
            helpId,
            helpKind,
            unknown5,
            passengerGlobalId,
            services,
            rewards,
            bonusRewards,
            helperHomeId
        )
    );
}
