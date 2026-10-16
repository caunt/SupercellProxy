using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>One truck order of another farm, as the server sends it; it carries the same fields as a saved order.</summary>
/// <param name="DataGlobalIds">Global ids of the goods the order asks for, matching <paramref name="Amounts"/> by position.</param>
/// <param name="Amounts">How many of each good the order asks for.</param>
/// <param name="Cash">Coins the order pays when delivered.</param>
/// <param name="Experience">Experience the order gives when delivered.</param>
/// <param name="Slot">Position of the order on the order board.</param>
/// <param name="Level">Player level the order was made for.</param>
/// <param name="New">Whether the order still shows its "new" badge.</param>
/// <param name="Voucher">Global id of the voucher the order gives; 0 when it gives none.</param>
/// <param name="CashExperienceMultiplier">Percentage that shifts the bonus reward from experience to coins.</param>
/// <param name="BonusRewardEnabled">Whether the order gives its event bonus reward.</param>
/// <param name="BonusEventId">Event that adds the bonus reward.</param>
/// <param name="BonusRewardGlobalId">Global id of the bonus reward; 0 when there is none.</param>
/// <param name="BonusCount">How many of the bonus reward the order gives.</param>
/// <param name="ReceiverGlobalId">Global id of the customer who placed the order.</param>
/// <param name="TimerRemaining">Time left on an empty slot's countdown until its next order arrives.</param>
/// <param name="TimerDuration">Full length of that countdown.</param>
/// <param name="ReviverAvatarId">Home of the player who filled the order for its owner; null when nobody did.</param>
/// <param name="HelperId">Id of the order's help request; 0 when the owner has not asked for help.</param>
/// <param name="HelperRewardData">Help value saved as <c>HRD</c> on the order; its meaning for orders is not identified.</param>
/// <param name="HelperCompleted">Help flag saved as <c>HC</c> on the order; its meaning for orders is not identified.</param>
/// <param name="HelperGranted">Help flag saved as <c>HG</c> on the order; its meaning for orders is not identified.</param>
/// <param name="PromotionReward">Promotion box reward attached to the order; null when there is none.</param>
/// <param name="SeasonalCurrencyBonus">Whether the bonus reward pays out in the event's seasonal currency.</param>
/// <param name="OrderSourceGlobalId">Global id of the table entry the order was generated from; null when not sent.</param>
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
