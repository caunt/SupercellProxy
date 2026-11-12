using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Contains the complete native derby task-board response.</summary>
public sealed record DerbyBoardMessage : CodecMessage<DerbyBoardMessage>, ICodecProvider<DerbyBoardMessage>
{
    /// <inheritdoc />
    public static ICodec<DerbyBoardMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(BoardCounters.Codec,
            static message => new BoardCounters(message.TaskLimit,
                message.RemainingTasks,
                message.DailyTasksRefillTime,
                message.Unknown0,
                message.Unknown1,
                message.ExtraTaskOfferValue),
            Codecs.OptionalLongId,
            static message => message.CurrentInstanceId,
            Codecs.Boolean,
            static message => message.Flag0,
            Codecs.Boolean,
            static message => message.Flag1,
            Codecs.Boolean,
            static message => message.Flag2,
            Codecs.OptionalLongId,
            static message => message.PreviousInstanceId,
            Codecs.NullableArray(DerbyTaskEntry.Codec, CollectionCount.VarInt.WithStrictAbsence()),
            static message => message.Tasks,
            Codecs.NullableArray(DerbyBoardAuxiliaryEntry.Codec, CollectionCount.VarInt.WithStrictAbsence()),
            static message => message.AuxiliaryEntries,
            Codecs.VarInt,
            static message => message.Unknown2,
            Codecs.VarInt,
            static message => message.Unknown3,
            Codecs.VarInt,
            static message => message.Unknown4,
            Codecs.VarInt,
            static message => message.Unknown5,
            Codecs.Boolean,
            static message => message.Flag3,
            Codecs.VarInt,
            static message => message.Unknown6,
            Codecs.Array(DerbyBoardGroupEntry.Codec, CollectionCount.Bounded),
            static message => message.Groups,
            Codecs.Optional(DerbyBoardSupplement.Codec),
            static message => message.Supplement,
            static (boardCounters,
                currentInstanceId,
                flag0,
                flag1,
                flag2,
                previousInstanceId,
                tasks,
                auxiliaryEntries,
                unknown2,
                unknown3,
                unknown4,
                unknown5,
                flag3,
                unknown6,
                groups,
                supplement) => new DerbyBoardMessage
                {
                    TaskLimit = boardCounters.TaskLimit,
                    RemainingTasks = boardCounters.RemainingTasks,
                    DailyTasksRefillTime = boardCounters.DailyTasksRefillTime,
                    Unknown0 = boardCounters.Unknown0,
                    Unknown1 = boardCounters.Unknown1,
                    ExtraTaskOfferValue = boardCounters.ExtraTaskOfferValue,
                    CurrentInstanceId = currentInstanceId,
                    Flag0 = flag0,
                    Flag1 = flag1,
                    Flag2 = flag2,
                    PreviousInstanceId = previousInstanceId,
                    Tasks = tasks,
                    AuxiliaryEntries = auxiliaryEntries,
                    Unknown2 = unknown2,
                    Unknown3 = unknown3,
                    Unknown4 = unknown4,
                    Unknown5 = unknown5,
                    Flag3 = flag3,
                    Unknown6 = unknown6,
                    Groups = groups,
                    Supplement = supplement,
                }),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Gets or sets the optional auxiliary board entries.</summary>
    /// <value>The five-value entries of the board's second collection, in wire order, or <see langword="null"/> when native wrote the -1 absent count; their meaning is not yet identified.</value>
    public DerbyBoardAuxiliaryEntry[]? AuxiliaryEntries { get; init; }
    /// <summary>Gets or sets the cached current derby race instance.</summary>
    /// <value>The identifier of the derby race the neighborhood is in now, or <see langword="null"/> when the presence flag is clear because no race is running.</value>
    public LongId? CurrentInstanceId { get; init; }
    /// <summary>Gets or sets the Unix time in seconds at which the daily task allowance refills, or minus one when none is scheduled.</summary>
    /// <value>Seconds since the Unix epoch of the next allowance refill, or -1 when no refill is pending; it is the third board counter on the wire.</value>
    public int DailyTasksRefillTime { get; init; }
    /// <summary>Gets or sets the offer value echoed when purchasing an additional task.</summary>
    /// <value>The opaque offer value to send back unchanged in <see cref="BuyExtraDerbyTaskMessage.OfferValue"/>; it is the last of the six leading board counters.</value>
    public int ExtraTaskOfferValue { get; init; }
    /// <summary>Gets or sets the first retained board flag.</summary>
    /// <value><see langword="true"/> if native set the first Boolean after <see cref="CurrentInstanceId"/>; otherwise, <see langword="false"/>.</value>
    /// <remarks>Its meaning is not yet identified.</remarks>
    public bool Flag0 { get; init; }
    /// <summary>Gets or sets the second retained board flag.</summary>
    /// <value><see langword="true"/> if native set the second Boolean after <see cref="CurrentInstanceId"/>; otherwise, <see langword="false"/>.</value>
    /// <remarks>Its meaning is not yet identified.</remarks>
    public bool Flag1 { get; init; }
    /// <summary>Gets or sets the third retained board flag.</summary>
    /// <value><see langword="true"/> if native set the Boolean just before <see cref="PreviousInstanceId"/>; otherwise, <see langword="false"/>.</value>
    /// <remarks>Its meaning is not yet identified.</remarks>
    public bool Flag2 { get; init; }
    /// <summary>Gets or sets the final retained board flag.</summary>
    /// <value><see langword="true"/> if native set the Boolean between <see cref="Unknown5"/> and <see cref="Unknown6"/>; otherwise, <see langword="false"/>.</value>
    /// <remarks>Its meaning is not yet identified.</remarks>
    public bool Flag3 { get; init; }
    /// <summary>Gets or sets the ordered board groups.</summary>
    /// <value>The group entries in wire order, each with its member identifiers; an empty array when the reply lists no groups, and at most 65536 entries.</value>
    public DerbyBoardGroupEntry[] Groups { get; init; } = [];
    /// <summary>Gets or sets the cached previous derby race instance.</summary>
    /// <value>The identifier of the race the neighborhood took part in before the current one, or <see langword="null"/> when the presence flag is clear.</value>
    public LongId? PreviousInstanceId { get; init; }
    /// <summary>Gets or sets the remaining task allowance supplied by the server.</summary>
    /// <value>How many more tasks the player may still take before the allowance refills at <see cref="DailyTasksRefillTime"/>.</value>
    public int RemainingTasks { get; init; }
    /// <summary>Gets or sets the optional board supplement.</summary>
    /// <value>The six-value block that closes the reply, or <see langword="null"/> when its presence flag is clear.</value>
    public DerbyBoardSupplement? Supplement { get; init; }
    /// <summary>Gets or sets the native task limit.</summary>
    /// <value>The total number of tasks the player may take in the allowance period, written as the first var-int of the reply.</value>
    public int TaskLimit { get; init; }
    /// <summary>Gets or sets the optional ordered task board.</summary>
    /// <value>The board's task slots in board order, or <see langword="null"/> when native wrote the -1 absent count; any other negative count is rejected as malformed.</value>
    public DerbyTaskEntry[]? Tasks { get; init; }
    /// <summary>Gets or sets the first retained board counter.</summary>
    /// <value>The var-int written right after <see cref="DailyTasksRefillTime"/>; its meaning is not yet identified, so it is carried through unchanged.</value>
    public int Unknown0 { get; init; }
    /// <summary>Gets or sets the second retained board counter.</summary>
    /// <value>The var-int written just before <see cref="ExtraTaskOfferValue"/>; its meaning is not yet identified.</value>
    public int Unknown1 { get; init; }
    /// <summary>Gets or sets the first retained value following auxiliary entries.</summary>
    /// <value>The var-int written right after <see cref="AuxiliaryEntries"/>; its meaning is not yet identified.</value>
    public int Unknown2 { get; init; }
    /// <summary>Gets or sets the second retained value following auxiliary entries.</summary>
    /// <value>The var-int written after <see cref="Unknown2"/>; its meaning is not yet identified.</value>
    public int Unknown3 { get; init; }
    /// <summary>Gets or sets the third retained value following auxiliary entries.</summary>
    /// <value>The var-int written after <see cref="Unknown3"/>; its meaning is not yet identified.</value>
    public int Unknown4 { get; init; }
    /// <summary>Gets or sets the fourth retained value following auxiliary entries.</summary>
    /// <value>The var-int written just before <see cref="Flag3"/>; its meaning is not yet identified.</value>
    public int Unknown5 { get; init; }
    /// <summary>Gets or sets the retained value preceding board groups.</summary>
    /// <value>The var-int written between <see cref="Flag3"/> and <see cref="Groups"/>; its meaning is not yet identified.</value>
    public int Unknown6 { get; init; }

    /// <summary>Omits identifiers and player details from diagnostic text.</summary>
    /// <returns>The type name <c>DerbyBoardMessage</c> alone, so instance identifiers, task holders and group members never reach logs.</returns>
    public override string ToString()
    {
        return nameof(DerbyBoardMessage);
    }

    // The leading board counters, read as one group to keep the field list within a composite.
    private readonly record struct BoardCounters(int TaskLimit, int RemainingTasks, int DailyTasksRefillTime, int Unknown0, int Unknown1, int ExtraTaskOfferValue)
    {
        public static ICodec<BoardCounters> Codec { get; } = Codecs.Composite(Codecs.VarInt,
            static segment => segment.TaskLimit,
            Codecs.VarInt,
            static segment => segment.RemainingTasks,
            Codecs.VarInt,
            static segment => segment.DailyTasksRefillTime,
            Codecs.VarInt,
            static segment => segment.Unknown0,
            Codecs.VarInt,
            static segment => segment.Unknown1,
            Codecs.VarInt,
            static segment => segment.ExtraTaskOfferValue,
            static (taskLimit, remainingTasks, dailyTasksRefillTime, unknown0, unknown1, extraTaskOfferValue) => new BoardCounters(taskLimit, remainingTasks, dailyTasksRefillTime, unknown0, unknown1, extraTaskOfferValue));
    }
}
