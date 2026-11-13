using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>State of a Valley obstacle task: something blocking a node that a player clears for a reward.</summary>
public sealed record MapGameObstacleTaskStatePayload : MapGameTaskStatePayload, ICodecProvider<MapGameObstacleTaskStatePayload>
{
    /// <summary>
    /// The native <c>TaskType</c> name of this payload.
    /// </summary>
    internal const string NativeTaskType = "Obstacle";

    /// <summary>Initializes a new instance of the <see cref="MapGameObstacleTaskStatePayload"/> record. Creates the state from its decoded fields.</summary>
    /// <param name="positive">The native <c>Positive</c> flag; its meaning is not identified.</param>
    /// <param name="helpRequestSent">Whether help was asked for the task.</param>
    /// <param name="helperAvatarId">Account of the player who helped; <see langword="null"/> when nobody did.</param>
    /// <param name="rewardData">Reward of the task; <see langword="null"/> when not drawn yet.</param>
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
    public static ICodec<MapGameObstacleTaskStatePayload> Codec { get; } = Codecs.Composite(Codecs.Boolean,
        static value => value.Positive,
        Codecs.Boolean,
        static value => value.HelpRequestSent,
        Codecs.OptionalLongId,
        static value => value.HelperAvatarId,
        Codecs.OptionalValue(Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount)),
        static value => value.RewardData,
        static (positive, helpRequestSent, helperAvatarId, rewardData) => new MapGameObstacleTaskStatePayload(positive, helpRequestSent, helperAvatarId, rewardData));

    /// <summary>
    /// Gets or sets a value indicating whether help was requested (native JSON <c>HelpRequestSent</c>).
    /// </summary>
    /// <value><see langword="true"/> if the obstacle's owner asked for help with it; otherwise, <see langword="false"/>.</value>
    public bool HelpRequestSent { get; init; }

    /// <summary>
    /// Gets or sets the helping avatar (native JSON <c>HelperAvatarID</c>).
    /// </summary>
    /// <value>The account id of the player who cleared the obstacle for its owner; <see langword="null"/> until someone helps, and only one helper is recorded.</value>
    [JsonPropertyName("HelperAvatarId")]
    public LongId? HelperAvatarId { get; init; }

    /// <summary>
    /// Gets the native <c>Positive</c> flag.
    /// </summary>
    /// <value><see langword="true"/> if the flag is set; otherwise, <see langword="false"/>.</value>
    /// <remarks>Its meaning is not yet identified.</remarks>
    public bool Positive { get; }

    /// <summary>
    /// Gets the reward granted for the task (native JSON <c>RewardData</c>).
    /// </summary>
    /// <value>Pairs of a reward row's global id and amount; <see langword="null"/> before the reward is drawn.</value>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? RewardData { get; }

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
