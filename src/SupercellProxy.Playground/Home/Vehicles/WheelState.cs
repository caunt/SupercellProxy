using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed record WheelState(
    GameObjectState GameObject,
    int State,
    int ChecksumState0,
    int LastInitDayIndex,
    int JackpotCount,
    int PrizeType,
    int PrizeGlobalId,
    int PrizeCount,
    int BoughtSpins,
    int NumSpins,
    int LastSpinDayIndex,
    int ConsecutiveSpinDays,
    int BoughtSpinsDaily,
    int FarmPassSpins,
    int AdsSpins,
    int[][] Prizes,
    int[][] Amounts,
    int SlotCount
)
{
    public static WheelState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom
    )
    {
        const string wheelCarsFile = "data/wheel_cars.csv";

        if (!dataTableResolver.TryGetTableId(wheelCarsFile, out var wheelCarTableId))
            throw new InvalidOperationException(
                $"{wheelCarsFile} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == wheelCarTableId)
            .Select(gameObject => Create(gameObject, dataTableResolver, constructorRandom))
            .ToArray();
    }

    private static WheelState Create(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom
    )
    {
        var snapshot = gameObject.Snapshot;
        var (slotCount, freeSpinSlot, jackpotSlot) = ResolveSlotConfiguration(
            gameObject,
            dataTableResolver
        );
        ValidatePrizeRows(gameObject, dataTableResolver, slotCount);
        ConsumeInitialShuffle(constructorRandom, slotCount, freeSpinSlot, jackpotSlot);

        return new WheelState(
            gameObject,
            unchecked(uint.CreateTruncating(snapshot.State)) < 2 ? 1 : snapshot.State,
            0,
            snapshot.LastInitDayIndex,
            snapshot.JackpotCount,
            snapshot.PrizeType,
            snapshot.PrizeGlobalID,
            snapshot.PrizeCount,
            snapshot.BoughtSpins,
            snapshot.NumSpins,
            snapshot.LastSpinDayIndex,
            snapshot.ConsecutiveSpinDays,
            snapshot.BoughtSpinsDaily,
            snapshot.FarmPassSpins,
            snapshot.AdsSpins,
            snapshot.WheelPrizes,
            snapshot.WheelAmounts,
            slotCount
        );
    }

    private static (int SlotCount, int FreeSpinSlot, int JackpotSlot) ResolveSlotConfiguration(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver
    )
    {
        if (
            !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "WheelNumSlots",
                out var slotCount
            )
            || !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "WheelFreeSpinSlot",
                out var freeSpinSlot
            )
            || !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "WheelJackpotSlot",
                out var jackpotSlot
            )
            || slotCount <= 0
            || freeSpinSlot < -1
            || freeSpinSlot >= slotCount
            || jackpotSlot < -1
            || jackpotSlot >= slotCount
        )
        {
            throw new InvalidDataException(
                $"Wheel {gameObject.Data.Name} has invalid native slot configuration."
            );
        }

        return (slotCount, freeSpinSlot, jackpotSlot);
    }

    private static void ValidatePrizeRows(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
        int slotCount
    )
    {
        var snapshot = gameObject.Snapshot;

        if (
            snapshot.WheelPrizes.Length is 0
            || snapshot.WheelPrizes.Length != snapshot.WheelAmounts.Length
        )
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Wheel {gameObject.GlobalId} has invalid prize rows."
                )
            );

        for (var row = 0; row < snapshot.WheelPrizes.Length; row++)
        {
            if (
                snapshot.WheelPrizes[row].Length != slotCount
                || snapshot.WheelAmounts[row].Length != slotCount
            )
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Wheel {gameObject.GlobalId} row {row} does not contain {slotCount} slots."
                    )
                );

            foreach (var prizeGlobalId in snapshot.WheelPrizes[row])
            {
                if (prizeGlobalId is not -1 && !dataTableResolver.TryResolve(prizeGlobalId, out _))
                    throw new InvalidDataException(
                        string.Create(
                            CultureInfo.InvariantCulture,
                            $"Wheel {gameObject.GlobalId} has unresolved prize {prizeGlobalId}."
                        )
                    );
            }
        }
    }

    private static void ConsumeInitialShuffle(
        GameRandom constructorRandom,
        int slotCount,
        int freeSpinSlot,
        int jackpotSlot
    )
    {
        var selectableSlotCount = Enumerable
            .Range(0, slotCount)
            .Count(slot => slot != freeSpinSlot && slot != jackpotSlot);

        for (var remaining = selectableSlotCount; remaining > 0; remaining--)
            _ = constructorRandom.NextInt(remaining);
    }
}
