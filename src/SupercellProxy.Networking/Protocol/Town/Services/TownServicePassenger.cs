using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>One town service passenger entry.</summary>
/// <param name="PassengerId">The passenger id.</param>
/// <param name="HelpId">The help id.</param>
/// <param name="State">The state.</param>
/// <param name="SlotIndex">The slot index.</param>
/// <param name="TimerStart">The timer start.</param>
/// <param name="TimerDuration">The timer duration.</param>
/// <param name="PassengerGlobalId">The passenger data id.</param>
/// <param name="Services">The services.</param>
/// <param name="Rewards">The rewards.</param>
/// <param name="BonusRewards">The bonus rewards.</param>
/// <param name="HelperHomeId">The helper home id.</param>
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
