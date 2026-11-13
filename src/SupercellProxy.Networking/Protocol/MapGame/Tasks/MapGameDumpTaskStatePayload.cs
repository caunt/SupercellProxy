using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>State of a Valley dump task: goods to drop off at a node for a reward.</summary>
public sealed record MapGameDumpTaskStatePayload : MapGameTaskStatePayload, ICodecProvider<MapGameDumpTaskStatePayload>
{
    /// <summary>
    /// The native <c>TaskType</c> name of this payload.
    /// </summary>
    internal const string NativeTaskType = "Dump";

    /// <summary>Initializes a new instance of the <see cref="MapGameDumpTaskStatePayload"/> record. Creates the state from its decoded fields.</summary>
    /// <param name="requiredGoods">Goods and amounts the task asks for.</param>
    /// <param name="rewardData">Reward of the task; <see langword="null"/> when not drawn yet.</param>
    /// <param name="helpRequestSent">Whether help was asked for the task.</param>
    /// <param name="helperAvatarId">Account of the player who helped; <see langword="null"/> when nobody did.</param>
    /// <param name="emojiGlobalId">Global id of the emoji shown on the task.</param>
    /// <param name="sanctuaryAnimalGlobalId">Global id of the task's sanctuary animal; 0 when it has none.</param>
    public MapGameDumpTaskStatePayload(ReadOnlyMemory<CommandDataReferenceVarIntPair> requiredGoods,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? rewardData,
        bool helpRequestSent,
        LongId? helperAvatarId,
        int emojiGlobalId,
        int sanctuaryAnimalGlobalId)
    {
        RequiredGoods = requiredGoods.ToArray();

        RewardData = rewardData is null
            ? null
            : (ReadOnlyMemory<CommandDataReferenceVarIntPair>?)rewardData.Value.ToArray();

        HelpRequestSent = helpRequestSent;
        HelperAvatarId = helperAvatarId;
        EmojiGlobalId = emojiGlobalId;
        SanctuaryAnimalGlobalId = sanctuaryAnimalGlobalId;
    }

    /// <inheritdoc />
    public static ICodec<MapGameDumpTaskStatePayload> Codec { get; } = Codecs.Composite(Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount),
        static value => value.RequiredGoods,
        Codecs.OptionalValue(Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount)),
        static value => value.RewardData,
        Codecs.Boolean,
        static value => value.HelpRequestSent,
        Codecs.OptionalLongId,
        static value => value.HelperAvatarId,
        Codecs.VarInt,
        static value => value.EmojiGlobalId,
        Codecs.VarInt,
        static value => value.SanctuaryAnimalGlobalId,
        static (requiredGoods, rewardData, helpRequestSent, helperAvatarId, emojiGlobalId, sanctuaryAnimalGlobalId) => new MapGameDumpTaskStatePayload(requiredGoods, rewardData, helpRequestSent, helperAvatarId, emojiGlobalId, sanctuaryAnimalGlobalId));

    /// <summary>
    /// Gets the emoji shown on the task (native JSON <c>EmojiData</c>).
    /// </summary>
    /// <value>The global id of a Valley emoji row displayed with the task; 0 when the task shows none.</value>
    [JsonPropertyName("EmojiGlobalId")]
    public int EmojiGlobalId { get; }

    /// <summary>
    /// Gets or sets a value indicating whether help was requested (native JSON <c>HelpRequestSent</c>).
    /// </summary>
    /// <value><see langword="true"/> if the task was marked for help; otherwise, <see langword="false"/>.</value>
    public bool HelpRequestSent { get; init; }

    /// <summary>
    /// Gets or sets the helping avatar (native JSON <c>HelperAvatarID</c>).
    /// </summary>
    /// <value>The account id of the player who marked the task; <see langword="null"/> until someone does.</value>
    [JsonPropertyName("HelperAvatarId")]
    public LongId? HelperAvatarId { get; init; }

    /// <summary>
    /// Gets the goods and quantities required to complete this dump task.
    /// </summary>
    /// <value>Pairs of a goods row's global id and a positive amount, each good listed once.</value>
    [JsonPropertyName("Values")]
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> RequiredGoods { get; }

    /// <summary>
    /// Gets the reward granted for the task (native JSON <c>RewardData</c>).
    /// </summary>
    /// <value>Pairs of a reward row's global id and amount; <see langword="null"/> when the wire marks it absent.</value>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? RewardData { get; }

    /// <summary>
    /// Gets the sanctuary animal of the task (native JSON <c>SanctuaryAnimalData</c>).
    /// </summary>
    /// <value>The global id of the sanctuary animal row tied to the task, the last field of the payload; 0 when the task involves no animal.</value>
    [JsonPropertyName("SanctuaryAnimalGlobalId")]
    public int SanctuaryAnimalGlobalId { get; }

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
