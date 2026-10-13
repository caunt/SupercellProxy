using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// A delivery task state, saved natively with the keys <c>Pickup</c>, <c>Delivery</c> and <c>Started</c>.
/// </summary>
/// <param name="Pickup">The pickup value saved under <c>Pickup</c>.</param>
/// <param name="Delivery">The delivery value saved under <c>Delivery</c>.</param>
/// <param name="Started">Whether the pawn picked up the delivery and is carrying it.</param>
public sealed record MapGameDeliveryTaskStatePayload(int Pickup, int Delivery, bool Started) : MapGameTaskStatePayload, ICodecProvider<MapGameDeliveryTaskStatePayload>
{
    /// <summary>
    /// The native <c>TaskType</c> name of this payload.
    /// </summary>
    internal const string NativeTaskType = "Delivery";

    /// <inheritdoc />
    public static ICodec<MapGameDeliveryTaskStatePayload> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Pickup,
        Codecs.VarInt,
        static value => value.Delivery,
        Codecs.Boolean,
        static value => value.Started,
        static (pickup, delivery, started) => new MapGameDeliveryTaskStatePayload(pickup, delivery, started)
    );

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
