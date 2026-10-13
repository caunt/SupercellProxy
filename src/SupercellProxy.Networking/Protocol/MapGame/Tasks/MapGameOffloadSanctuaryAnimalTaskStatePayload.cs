using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// An offload-sanctuary-animal task state, saved natively with the key <c>DiamondPrice</c>.
/// </summary>
/// <param name="DiamondPrice">The diamond price of offloading the animal.</param>
public sealed record MapGameOffloadSanctuaryAnimalTaskStatePayload(int DiamondPrice) : MapGameTaskStatePayload, ICodecProvider<MapGameOffloadSanctuaryAnimalTaskStatePayload>
{
    /// <summary>
    /// The native <c>TaskType</c> name of this payload.
    /// </summary>
    internal const string NativeTaskType = "Offload Sanctuary Animal";

    /// <inheritdoc />
    public static ICodec<MapGameOffloadSanctuaryAnimalTaskStatePayload> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.DiamondPrice,
        static (diamondPrice) => new MapGameOffloadSanctuaryAnimalTaskStatePayload(diamondPrice)
    );

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
