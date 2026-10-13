using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameObstacleTaskStatePayload</c>.
/// </summary>
public sealed record MapGameObstacleTaskStatePayload : MapGameTaskStatePayload, ICodecProvider<MapGameObstacleTaskStatePayload>
{
    /// <summary>
    /// The native <c>TaskType</c> name of this payload.
    /// </summary>
    internal const string NativeTaskType = "Obstacle";

    /// <summary>
    /// Initializes a new <see cref="MapGameObstacleTaskStatePayload"/> instance.
    /// </summary>
    public MapGameObstacleTaskStatePayload(bool positive, bool helpRequestSent, LongId? helperAvatarId, ReadOnlyMemory<CommandDataReferenceVarIntPair>? rewardData)
    {
        Positive = positive;
        HelpRequestSent = helpRequestSent;
        HelperAvatarId = helperAvatarId;
        RewardData = rewardData is null
            ? null
            : (ReadOnlyMemory<CommandDataReferenceVarIntPair>?)rewardData.Value.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameObstacleTaskStatePayload> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static value => value.Positive,
        Codecs.Boolean,
        static value => value.HelpRequestSent,
        Codecs.OptionalLongId,
        static value => value.HelperAvatarId,
        Codecs.OptionalValue(Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount)),
        static value => value.RewardData,
        static (positive, helpRequestSent, helperAvatarId, rewardData) => new MapGameObstacleTaskStatePayload(positive, helpRequestSent, helperAvatarId, rewardData)
    );

    /// <summary>
    /// Gets a value indicating whether help was requested (native JSON <c>HelpRequestSent</c>).
    /// </summary>
    public bool HelpRequestSent { get; init; }

    /// <summary>
    /// Gets the helping avatar (native JSON <c>HelperAvatarID</c>).
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HelperAvatarId")]
    public LongId? HelperAvatarId { get; init; }

    /// <summary>
    /// Gets the native <c>Positive</c> flag.
    /// </summary>
    public bool Positive { get; }

    /// <summary>
    /// Gets the reward granted for the task (native JSON <c>RewardData</c>).
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? RewardData { get; }

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
