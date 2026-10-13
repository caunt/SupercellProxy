using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>
/// The map game's daily spawn and delivery counters and its chicken log, saved natively with the keys <c>CurrentDayIndex</c>,
/// <c>DailySocialTaskSpawnCount</c>, <c>LastDailySocialTaskSpawnTime</c>, <c>DailyChickenSpawnCount</c>, <c>LastDailyChickenTaskSpawnTime</c>,
/// <c>ChickenLogEntries</c>, <c>DailySanctuaryAnimalDeliveredCount</c>, <c>LastDailySanctuaryAnimalDeliveredTime</c>,
/// <c>CollectedChickenCount</c> and <c>DeliveredSanctuaryAnimalCount</c> (save 0x100675460, decode 0x100675238).
/// </para>
/// </summary>
/// <param name="CurrentDayIndex">The day index the daily counters belong to.</param>
/// <param name="DailySocialTaskSpawnCount">How many social tasks spawned today.</param>
/// <param name="DailyChickenSpawnCount">How many chicken tasks spawned today.</param>
/// <param name="LastDailyChickenTaskSpawnTime">When the last chicken task spawned.</param>
/// <param name="DailySanctuaryAnimalDeliveredCount">How many sanctuary animals were delivered today.</param>
/// <param name="LastDailySanctuaryAnimalDeliveredTime">When the last sanctuary animal was delivered.</param>
/// <param name="ChickenLogEntries">The collected-chicken and delivered-animal contributions.</param>
/// <param name="CollectedChickenCount">The total collected chickens.</param>
/// <param name="DeliveredSanctuaryAnimalCount">The total delivered sanctuary animals.</param>
/// <param name="LastDailySocialTaskSpawnTime">When the last social task spawned.</param>
public sealed record MapGameConfiguration(
    int CurrentDayIndex,
    int DailySocialTaskSpawnCount,
    int DailyChickenSpawnCount,
    int LastDailyChickenTaskSpawnTime,
    int DailySanctuaryAnimalDeliveredCount,
    int LastDailySanctuaryAnimalDeliveredTime,
    ReadOnlyMemory<MapGameChickenLogEntry> ChickenLogEntries,
    int CollectedChickenCount,
    int DeliveredSanctuaryAnimalCount,
    int LastDailySocialTaskSpawnTime
) : ICodecProvider<MapGameConfiguration>
{
    /// <inheritdoc />
    public static ICodec<MapGameConfiguration> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static configuration => configuration.CurrentDayIndex,
        Codecs.VarInt,
        static configuration => configuration.DailySocialTaskSpawnCount,
        Codecs.VarInt,
        static configuration => configuration.DailyChickenSpawnCount,
        Codecs.VarInt,
        static configuration => configuration.LastDailyChickenTaskSpawnTime,
        Codecs.VarInt,
        static configuration => configuration.DailySanctuaryAnimalDeliveredCount,
        Codecs.VarInt,
        static configuration => configuration.LastDailySanctuaryAnimalDeliveredTime,
        Codecs.Memory(MapGameChickenLogEntry.Codec),
        static configuration => configuration.ChickenLogEntries,
        Codecs.VarInt,
        static configuration => configuration.CollectedChickenCount,
        Codecs.VarInt,
        static configuration => configuration.DeliveredSanctuaryAnimalCount,
        Codecs.VarInt,
        static configuration => configuration.LastDailySocialTaskSpawnTime,
        static (
            currentDayIndex,
            dailySocialTaskSpawnCount,
            dailyChickenSpawnCount,
            lastDailyChickenTaskSpawnTime,
            dailySanctuaryAnimalDeliveredCount,
            lastDailySanctuaryAnimalDeliveredTime,
            chickenLogEntries,
            collectedChickenCount,
            deliveredSanctuaryAnimalCount,
            lastDailySocialTaskSpawnTime
        ) => new MapGameConfiguration(
            currentDayIndex,
            dailySocialTaskSpawnCount,
            dailyChickenSpawnCount,
            lastDailyChickenTaskSpawnTime,
            dailySanctuaryAnimalDeliveredCount,
            lastDailySanctuaryAnimalDeliveredTime,
            chickenLogEntries,
            collectedChickenCount,
            deliveredSanctuaryAnimalCount,
            lastDailySocialTaskSpawnTime
        )
    );
}
