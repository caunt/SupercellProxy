using System.Globalization;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record OrderState(
    int Slot,
    DataTableReference[] Items,
    int[] Amounts,
    int Cash,
    int Experience,
    int Level,
    bool IsNew,
    int Voucher,
    int CashExperienceMultiplier,
    bool BonusRewardEnabled,
    int BonusEventId,
    DataTableReference? BonusReward,
    int BonusCount,
    DataTableReference Receiver,
    TimerSnapshot Timer,
    string? ReviverAvatarId,
    int ChecksumState0,
    int ChecksumState1,
    bool ChecksumFlag0,
    bool ChecksumFlag1,
    bool HasSeasonalCurrency,
    bool ChecksumFlag2
)
{
    public static OrderState Create(
        int slot,
        OrderSnapshot snapshot,
        DataTableResolver dataTableResolver
    )
    {
        if (snapshot.Data.Count is not 0)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Order slot {slot} contains unsupported fields: {string.Join(", ", snapshot.Data.Keys)}."
                )
            );

        if (snapshot.Datas.Length != snapshot.Amounts.Length)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Order slot {slot} has mismatched Datas and Amounts lengths."
                )
            );

        if (snapshot.Lvl < 1)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Order slot {slot} has invalid Lvl {snapshot.Lvl}."
                )
            );

        var items = ResolveItems(slot, snapshot.Datas, dataTableResolver);
        var receiver = ResolveReceiver(slot, snapshot.Receiver, dataTableResolver);

        return new OrderState(
            slot,
            items,
            snapshot.Amounts,
            snapshot.Cash,
            snapshot.Exp,
            snapshot.Lvl,
            IsNew: false,
            snapshot.Voucher,
            snapshot.CashExpMultiplier,
            BonusRewardEnabled: false,
            0,
            BonusReward: null,
            0,
            receiver,
            default,
            ReviverAvatarId: null,
            0,
            0,
            ChecksumFlag0: false,
            ChecksumFlag1: false,
            HasSeasonalCurrency: false,
            ChecksumFlag2: false
        );
    }

    private static DataTableReference[] ResolveItems(
        int slot,
        int[] dataIds,
        DataTableResolver resolver
    )
    {
        var items = new DataTableReference[dataIds.Length];
        for (var index = 0; index < dataIds.Length; index++)
        {
            if (!resolver.TryResolve(dataIds[index], out var item))
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Order slot {slot} has unresolved Datas value {dataIds[index]}."
                    )
                );
            items[index] = item;
        }
        return items;
    }

    private static DataTableReference ResolveReceiver(
        int slot,
        int receiverId,
        DataTableResolver resolver
    )
    {
        return resolver.TryResolve(receiverId, out var receiver)
            ? receiver
            : throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Order slot {slot} has unresolved Receiver {receiverId}."
                )
            );
    }
}
