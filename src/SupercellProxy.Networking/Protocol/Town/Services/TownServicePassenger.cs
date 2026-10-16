using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>A passenger at a town service building, as the server reports it after service help.</summary>
/// <param name="PassengerId">Id of the passenger's game object.</param>
/// <param name="HelpId">Id of the passenger's help request; 0 when none was made.</param>
/// <param name="State">Where the passenger is in its visit; the values are not identified for this command.</param>
/// <param name="SlotIndex">Service slot of the building the passenger uses.</param>
/// <param name="TimerStart">When the passenger's service started; its unit is not identified.</param>
/// <param name="TimerDuration">How long the passenger's service takes; its unit is not identified.</param>
/// <param name="PassengerGlobalId">Global id of the passenger's table entry.</param>
/// <param name="Services">Services the passenger wants, with what each one needs.</param>
/// <param name="Rewards">What the passenger pays for its services.</param>
/// <param name="BonusRewards">Extra reward the passenger gives once all its services are done.</param>
/// <param name="HelperHomeId">Home of the player who helped the service; null when nobody has.</param>
public sealed record TownServicePassenger(
    int PassengerId,
    int HelpId,
    int State,
    int SlotIndex,
    int TimerStart,
    int TimerDuration,
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
        static value => value.PassengerId,
        Codecs.VarInt,
        static value => value.HelpId,
        Codecs.VarInt,
        static value => value.State,
        Codecs.VarInt,
        static value => value.SlotIndex,
        Codecs.VarInt,
        static value => value.TimerStart,
        Codecs.VarInt,
        static value => value.TimerDuration,
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
            passengerId,
            helpId,
            state,
            slotIndex,
            timerStart,
            timerDuration,
            passengerGlobalId,
            services,
            rewards,
            bonusRewards,
            helperHomeId
        ) => new TownServicePassenger(
            passengerId,
            helpId,
            state,
            slotIndex,
            timerStart,
            timerDuration,
            passengerGlobalId,
            services,
            rewards,
            bonusRewards,
            helperHomeId
        )
    );
}
