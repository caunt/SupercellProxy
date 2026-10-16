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

    /// <summary>Creates the state from its decoded fields.</summary>
    /// <param name="requiredGoods">Goods and amounts the task asks for.</param>
    /// <param name="rewardData">Reward of the task; null when not drawn yet.</param>
    /// <param name="helpRequestSent">Whether help was asked for the task.</param>
    /// <param name="helperAvatarId">Account of the player who helped; null when nobody did.</param>
    /// <param name="emojiGlobalId">Global id of the emoji shown on the task.</param>
    /// <param name="sanctuaryAnimalGlobalId">Global id of the task's sanctuary animal; 0 when it has none.</param>
    public MapGameDumpTaskStatePayload(
        ReadOnlyMemory<CommandDataReferenceVarIntPair> requiredGoods,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? rewardData,
        bool helpRequestSent,
        LongId? helperAvatarId,
        int emojiGlobalId,
        int sanctuaryAnimalGlobalId
    )
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
    public static ICodec<MapGameDumpTaskStatePayload> Codec { get; } = Codecs.Composite(
        Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount),
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
        static (requiredGoods, rewardData, helpRequestSent, helperAvatarId, emojiGlobalId, sanctuaryAnimalGlobalId) => new MapGameDumpTaskStatePayload(requiredGoods, rewardData, helpRequestSent, helperAvatarId, emojiGlobalId, sanctuaryAnimalGlobalId)
    );

    /// <summary>
    /// Gets the emoji shown on the task (native JSON <c>EmojiData</c>).
    /// </summary>
    [JsonPropertyName("EmojiGlobalId")]
    public int EmojiGlobalId { get; }

    /// <summary>
    /// Gets a value indicating whether help was requested (native JSON <c>HelpRequestSent</c>).
    /// </summary>
    public bool HelpRequestSent { get; init; }

    /// <summary>
    /// Gets the helping avatar (native JSON <c>HelperAvatarID</c>).
    /// </summary>
    [JsonPropertyName("HelperAvatarId")]
    public LongId? HelperAvatarId { get; init; }

    /// <summary>
    /// Gets the goods and quantities required to complete this dump task.
    /// </summary>
    [JsonPropertyName("Values")]
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> RequiredGoods { get; }

    /// <summary>
    /// Gets the reward granted for the task (native JSON <c>RewardData</c>).
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? RewardData { get; }

    /// <summary>
    /// Gets the sanctuary animal of the task (native JSON <c>SanctuaryAnimalData</c>).
    /// </summary>
    [JsonPropertyName("SanctuaryAnimalGlobalId")]
    public int SanctuaryAnimalGlobalId { get; }

    /// <inheritdoc />
    public override string TaskType => NativeTaskType;
}
