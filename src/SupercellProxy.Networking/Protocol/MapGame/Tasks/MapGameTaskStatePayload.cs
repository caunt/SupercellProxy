using System.Collections.Frozen;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// The task-type specific part of a map-game task state; native selects its layout from the task data's <c>TaskType</c>.
/// </summary>
public abstract record MapGameTaskStatePayload
{
    /// <summary>
    /// Gets the native <c>TaskType</c> name that selects this payload layout.
    /// </summary>
    /// <value>The <c>TaskType</c> column value this payload type is registered under, such as <c>Obstacle</c> or <c>Gas Station</c>; writing a state uses it to pick the codec.</value>
    public abstract string TaskType { get; }

    private static FrozenDictionary<string, ICodec<MapGameTaskStatePayload>> Variants => field ??= new Dictionary<string, ICodec<MapGameTaskStatePayload>>(StringComparer.Ordinal)
    {
        [MapGameDumpTaskStatePayload.NativeTaskType] = Codecs.Subtype<MapGameTaskStatePayload, MapGameDumpTaskStatePayload>(MapGameDumpTaskStatePayload.Codec),
        [MapGameDeliveryTaskStatePayload.NativeTaskType] = Codecs.Subtype<MapGameTaskStatePayload, MapGameDeliveryTaskStatePayload>(MapGameDeliveryTaskStatePayload.Codec),
        [MapGameObstacleTaskStatePayload.NativeTaskType] = Codecs.Subtype<MapGameTaskStatePayload, MapGameObstacleTaskStatePayload>(MapGameObstacleTaskStatePayload.Codec),
        [MapGameChickenTaskStatePayload.NativeTaskType] = Codecs.Subtype<MapGameTaskStatePayload, MapGameChickenTaskStatePayload>(MapGameChickenTaskStatePayload.Codec),
        [MapGameSanctuaryAnimalTaskStatePayload.NativeTaskType] =
            Codecs.Subtype<MapGameTaskStatePayload, MapGameSanctuaryAnimalTaskStatePayload>(MapGameSanctuaryAnimalTaskStatePayload.Codec),
        [MapGameGasStationTaskStatePayload.NativeTaskType] = Codecs.Subtype<MapGameTaskStatePayload, MapGameGasStationTaskStatePayload>(MapGameGasStationTaskStatePayload.Codec),
        [MapGameOffloadSanctuaryAnimalTaskStatePayload.NativeTaskType] =
            Codecs.Subtype<MapGameTaskStatePayload, MapGameOffloadSanctuaryAnimalTaskStatePayload>(MapGameOffloadSanctuaryAnimalTaskStatePayload.Codec),
    }.ToFrozenDictionary(StringComparer.Ordinal);

    /// <summary>
    /// Gets the payload codec for a native task type name.
    /// </summary>
    internal static ICodec<MapGameTaskStatePayload> For(string taskType)
    {
        return Variants.TryGetValue(taskType, out ICodec<MapGameTaskStatePayload>? codec)
            ? codec
            : throw new NotSupportedException($"Map-game task type '{taskType}' is not supported.");
    }
}
