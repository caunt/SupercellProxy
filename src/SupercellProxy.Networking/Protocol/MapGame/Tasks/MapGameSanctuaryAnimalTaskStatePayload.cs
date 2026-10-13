using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameSanctuaryAnimalTaskStatePayload</c>.
/// </summary>
/// <param name="Pickup">The pickup node (native JSON <c>Pickup</c>).</param>
/// <param name="DeliveryNodeType">The delivery node type (native JSON <c>DeliveryNodeType</c>).</param>
/// <param name="AnimalGlobalId">The sanctuary-animal definition.</param>
/// <param name="Collected">Whether a pawn has collected the animal (native JSON <c>Started</c>).</param>
/// <param name="EmoteVisible">Whether the animal's emote is visible (native JSON <c>EmoteVisible</c>).</param>
/// <param name="Escaped">Whether the collected animal has escaped.</param>
/// <param name="EscapeNodeId">The node selected for the escape.</param>
/// <param name="EscapeCount">The retained wrapping escape counter.</param>
/// <param name="CollectorId">The collecting pawn's avatar id.</param>
public sealed record MapGameSanctuaryAnimalTaskStatePayload(
    int Pickup,
    int DeliveryNodeType,
    int AnimalGlobalId,
    bool Collected,
    bool EmoteVisible,
    bool Escaped,
    int EscapeNodeId,
    int EscapeCount,
    LongId? CollectorId
) : MapGameTaskStatePayload, ICodecProvider<MapGameSanctuaryAnimalTaskStatePayload>
{
    /// <summary>
    /// The native <c>TaskType</c> name of this payload.
    /// </summary>
    internal const string NativeTaskType = "Sanctuary Animal";

    /// <inheritdoc />
    public static ICodec<MapGameSanctuaryAnimalTaskStatePayload> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Pickup,
        Codecs.VarInt,
        static value => value.DeliveryNodeType,
        Codecs.VarInt,
        static value => value.AnimalGlobalId,
        Codecs.Boolean,
        static value => value.Collected,
        Codecs.Boolean,
        static value => value.EmoteVisible,
        Codecs.Boolean,
        static value => value.Escaped,
        Codecs.VarInt,
        static value => value.EscapeNodeId,
        Codecs.VarInt,
        static value => value.EscapeCount,
        Codecs.OptionalLongId,
        static value => value.CollectorId,
        static (pickup, deliveryNodeType, animalGlobalId, collected, emoteVisible, escaped, escapeNodeId, escapeCount, collectorId) => new MapGameSanctuaryAnimalTaskStatePayload(pickup, deliveryNodeType, animalGlobalId, collected, emoteVisible, escaped, escapeNodeId, escapeCount, collectorId)
    );

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
