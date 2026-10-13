using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Releases;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>One remote order entry.</summary>
/// <param name="DataGlobalIds">The data data ids.</param>
/// <param name="Amounts">The amounts.</param>
/// <param name="Cash">The cash.</param>
/// <param name="Experience">The experience.</param>
/// <param name="Slot">The slot.</param>
/// <param name="Level">The level.</param>
/// <param name="New">The new.</param>
/// <param name="Voucher">The voucher.</param>
/// <param name="CashExperienceMultiplier">The cash experience multiplier.</param>
/// <param name="BonusRewardEnabled">The bonus reward enabled.</param>
/// <param name="BonusEventId">The bonus event id.</param>
/// <param name="BonusRewardGlobalId">The bonus reward data id.</param>
/// <param name="BonusCount">The bonus count.</param>
/// <param name="ReceiverGlobalId">The receiver data id.</param>
/// <param name="TimerRemaining">The timer remaining.</param>
/// <param name="TimerDuration">The timer duration.</param>
/// <param name="ReviverAvatarId">The reviver avatar id.</param>
/// <param name="HelperId">The helper id.</param>
/// <param name="HelperRewardData">The helper reward data.</param>
/// <param name="HelperCompleted">The helper completed.</param>
/// <param name="HelperGranted">The helper granted.</param>
/// <param name="PromotionReward">The promotion reward.</param>
/// <param name="SeasonalCurrencyBonus">The seasonal currency bonus.</param>
/// <param name="OrderSourceGlobalId">The order source data id.</param>
public sealed record RemoteOrder(
    ReadOnlyMemory<int> DataGlobalIds,
    ReadOnlyMemory<int> Amounts,
    int Cash,
    int Experience,
    int Slot,
    int Level,
    bool New,
    int Voucher,
    int CashExperienceMultiplier,
    bool BonusRewardEnabled,
    int BonusEventId,
    int BonusRewardGlobalId,
    int BonusCount,
    int ReceiverGlobalId,
    int TimerRemaining,
    int TimerDuration,
    ReviverAvatar? ReviverAvatarId,
    int HelperId,
    int HelperRewardData,
    bool HelperCompleted,
    bool HelperGranted,
    OrderPromotionReward? PromotionReward,
    bool SeasonalCurrencyBonus,
    int? OrderSourceGlobalId
) : ICodecProvider<RemoteOrder>
{
    /// <inheritdoc />
    public static ICodec<RemoteOrder> Codec { get; } = Codecs.Composite(
        Codecs.Memory(Codecs.VarInt),
        static order => order.DataGlobalIds,
        Codecs.Memory(Codecs.VarInt),
        static order => order.Amounts,
        Rewards.Codec,
        static order => new Rewards(order.Cash, order.Experience),
        Codecs.VarInt,
        static order => order.Slot,
        Codecs.VarInt,
        static order => order.Level,
        Codecs.Boolean,
        static order => order.New,
        Codecs.VarInt,
        static order => order.Voucher,
        Codecs.VarInt,
        static order => order.CashExperienceMultiplier,
        Bonus.Codec,
        static order => new Bonus(order.BonusRewardEnabled, order.BonusEventId, order.BonusRewardGlobalId, order.BonusCount),
        Codecs.VarInt,
        static order => order.ReceiverGlobalId,
        Timer.Codec,
        static order => new Timer(order.TimerRemaining, order.TimerDuration),
        Codecs.Optional(ReviverAvatar.Codec),
        static order => order.ReviverAvatarId,
        Helper.Codec,
        static order => new Helper(order.HelperId, order.HelperRewardData, order.HelperCompleted, order.HelperGranted),
        Codecs.Optional(OrderPromotionReward.Codec),
        static order => order.PromotionReward,
        Codecs.Boolean,
        static order => order.SeasonalCurrencyBonus,
        Codecs.Since(Release17381.Version, OrderSourceCodec(), absent: null),
        static order => order.OrderSourceGlobalId,
        static (
            dataGlobalIds,
            amounts,
            rewards,
            slot,
            level,
            isNew,
            voucher,
            cashExperienceMultiplier,
            bonus,
            receiverGlobalId,
            timer,
            reviverAvatarId,
            helper,
            promotionReward,
            seasonalCurrencyBonus,
            orderSourceGlobalId
        ) => new RemoteOrder(
            dataGlobalIds,
            amounts,
            rewards.Cash,
            rewards.Experience,
            slot,
            level,
            isNew,
            voucher,
            cashExperienceMultiplier,
            bonus.BonusRewardEnabled,
            bonus.BonusEventId,
            bonus.BonusRewardGlobalId,
            bonus.BonusCount,
            receiverGlobalId,
            timer.TimerRemaining,
            timer.TimerDuration,
            reviverAvatarId,
            helper.HelperId,
            helper.HelperRewardData,
            helper.HelperCompleted,
            helper.HelperGranted,
            promotionReward,
            seasonalCurrencyBonus,
            orderSourceGlobalId
        )
    );

    // 1.73.81 appends the order source; native writes it only when the order has one.
    private static ICodec<int?> OrderSourceCodec()
    {
        return Codecs.Create<int?>(
            static stream => stream.ReadVarInt(),
            static (stream, orderSourceGlobalId) =>
            {
                if (orderSourceGlobalId is { } value)
                    stream.WriteVarInt(value);
            }
        );
    }

    // The bonus reward.
    private readonly record struct Bonus(bool BonusRewardEnabled, int BonusEventId, int BonusRewardGlobalId, int BonusCount)
    {
        public static ICodec<Bonus> Codec { get; } = Codecs.Composite(
            Codecs.Boolean,
            static segment => segment.BonusRewardEnabled,
            Codecs.VarInt,
            static segment => segment.BonusEventId,
            Codecs.VarInt,
            static segment => segment.BonusRewardGlobalId,
            Codecs.VarInt,
            static segment => segment.BonusCount,
            static (bonusRewardEnabled, bonusEventId, bonusRewardGlobalId, bonusCount) => new Bonus(bonusRewardEnabled, bonusEventId, bonusRewardGlobalId, bonusCount)
        );
    }

    // The helper state.
    private readonly record struct Helper(int HelperId, int HelperRewardData, bool HelperCompleted, bool HelperGranted)
    {
        public static ICodec<Helper> Codec { get; } = Codecs.Composite(
            Codecs.VarInt,
            static segment => segment.HelperId,
            Codecs.VarInt,
            static segment => segment.HelperRewardData,
            Codecs.Boolean,
            static segment => segment.HelperCompleted,
            Codecs.Boolean,
            static segment => segment.HelperGranted,
            static (helperId, helperRewardData, helperCompleted, helperGranted) => new Helper(helperId, helperRewardData, helperCompleted, helperGranted)
        );
    }

    // The cash and experience rewards.
    private readonly record struct Rewards(int Cash, int Experience)
    {
        public static ICodec<Rewards> Codec { get; } = Codecs.Composite(
            Codecs.VarInt,
            static segment => segment.Cash,
            Codecs.VarInt,
            static segment => segment.Experience,
            static (cash, experience) => new Rewards(cash, experience)
        );
    }

    // The order timer.
    private readonly record struct Timer(int TimerRemaining, int TimerDuration)
    {
        public static ICodec<Timer> Codec { get; } = Codecs.Composite(
            Codecs.VarInt,
            static segment => segment.TimerRemaining,
            Codecs.VarInt,
            static segment => segment.TimerDuration,
            static (timerRemaining, timerDuration) => new Timer(timerRemaining, timerDuration)
        );
    }
}
