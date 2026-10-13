using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameChickenTaskStatePayload</c>.
/// </summary>
public sealed record MapGameChickenTaskStatePayload : MapGameTaskStatePayload, ICodecProvider<MapGameChickenTaskStatePayload>
{
    /// <summary>
    /// The native <c>TaskType</c> name of this payload.
    /// </summary>
    internal const string NativeTaskType = "Chicken";

    /// <summary>
    /// Initializes a new <see cref="MapGameChickenTaskStatePayload"/> instance.
    /// </summary>
    public MapGameChickenTaskStatePayload(
        int requiredCollectorCount,
        bool helpRequestSent,
        LongId? helperAvatarId,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? rewardData,
        ReadOnlyMemory<LongId> collectorAvatarIds
    )
    {
        RequiredCollectorCount = requiredCollectorCount;
        HelpRequestSent = helpRequestSent;
        HelperAvatarId = helperAvatarId;
        RewardData = rewardData is null
            ? null
            : (ReadOnlyMemory<CommandDataReferenceVarIntPair>?)rewardData.Value.ToArray();
        CollectorAvatarIds = collectorAvatarIds.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameChickenTaskStatePayload> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.RequiredCollectorCount,
        Codecs.Boolean,
        static value => value.HelpRequestSent,
        Codecs.OptionalLongId,
        static value => value.HelperAvatarId,
        Codecs.OptionalValue(Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount)),
        static value => value.RewardData,
        Codecs.Memory(Codecs.LongId),
        static value => value.CollectorAvatarIds,
        static (requiredCollectorCount, helpRequestSent, helperAvatarId, rewardData, collectorAvatarIds) => new MapGameChickenTaskStatePayload(requiredCollectorCount, helpRequestSent, helperAvatarId, rewardData, collectorAvatarIds)
    );

    /// <summary>
    /// Gets the avatars that collected (native JSON <c>CollectorAvatarIDs</c>).
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("CollectorAvatarIds")]
    public ReadOnlyMemory<LongId> CollectorAvatarIds { get; init; }

    /// <summary>
    /// Gets a value indicating whether help was requested (native JSON <c>HelpRequestSent</c>).
    /// </summary>
    public bool HelpRequestSent { get; }

    /// <summary>
    /// Gets the helping avatar (native JSON <c>HelperAvatarID</c>).
    /// </summary>
    public LongId? HelperAvatarId { get; }

    /// <summary>
    /// Gets the number of collectors the task needs (native JSON <c>RequiredCollectorCount</c>).
    /// </summary>
    public int RequiredCollectorCount { get; }

    /// <summary>
    /// Gets the reward granted for the task (native JSON <c>RewardData</c>).
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? RewardData { get; }

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
