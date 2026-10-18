using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Contains the complete native derby task-board response.</summary>
public sealed record DerbyBoardMessage : CodecMessage<DerbyBoardMessage>, ICodecProvider<DerbyBoardMessage>
{
    /// <inheritdoc />
    public static ICodec<DerbyBoardMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            BoardCounters.Codec,
            static message => new BoardCounters(
                message.TaskLimit,
                message.RemainingTasks,
                message.DailyTasksRefillTime,
                message.Unknown0,
                message.Unknown1,
                message.ExtraTaskOfferValue
            ),
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
            static (
                boardCounters,
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
                supplement
            ) => new DerbyBoardMessage
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
            }
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Gets the optional auxiliary board entries.</summary>
    public DerbyBoardAuxiliaryEntry[]? AuxiliaryEntries { get; init; }
    /// <summary>Gets the cached current derby race instance.</summary>
    public LongId? CurrentInstanceId { get; init; }
    /// <summary>Gets the Unix time in seconds at which the daily task allowance refills, or minus one when none is scheduled.</summary>
    public int DailyTasksRefillTime { get; init; }
    /// <summary>Gets the offer value echoed when purchasing an additional task.</summary>
    public int ExtraTaskOfferValue { get; init; }
    /// <summary>Gets the first retained board flag.</summary>
    public bool Flag0 { get; init; }
    /// <summary>Gets the second retained board flag.</summary>
    public bool Flag1 { get; init; }
    /// <summary>Gets the third retained board flag.</summary>
    public bool Flag2 { get; init; }
    /// <summary>Gets the final retained board flag.</summary>
    public bool Flag3 { get; init; }
    /// <summary>Gets the ordered board groups.</summary>
    public DerbyBoardGroupEntry[] Groups { get; init; } = [];
    /// <summary>Gets the cached previous derby race instance.</summary>
    public LongId? PreviousInstanceId { get; init; }
    /// <summary>Gets the remaining task allowance supplied by the server.</summary>
    public int RemainingTasks { get; init; }
    /// <summary>Gets the optional board supplement.</summary>
    public DerbyBoardSupplement? Supplement { get; init; }
    /// <summary>Gets the native task limit.</summary>
    public int TaskLimit { get; init; }
    /// <summary>Gets the optional ordered task board.</summary>
    public DerbyTaskEntry[]? Tasks { get; init; }
    /// <summary>Gets the first retained board counter.</summary>
    public int Unknown0 { get; init; }
    /// <summary>Gets the second retained board counter.</summary>
    public int Unknown1 { get; init; }
    /// <summary>Gets the first retained value following auxiliary entries.</summary>
    public int Unknown2 { get; init; }
    /// <summary>Gets the second retained value following auxiliary entries.</summary>
    public int Unknown3 { get; init; }
    /// <summary>Gets the third retained value following auxiliary entries.</summary>
    public int Unknown4 { get; init; }
    /// <summary>Gets the fourth retained value following auxiliary entries.</summary>
    public int Unknown5 { get; init; }
    /// <summary>Gets the retained value preceding board groups.</summary>
    public int Unknown6 { get; init; }

    /// <summary>Omits identifiers and player details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyBoardMessage);
    }

    // The leading board counters, read as one group to keep the field list within a composite.
    private readonly record struct BoardCounters(int TaskLimit, int RemainingTasks, int DailyTasksRefillTime, int Unknown0, int Unknown1, int ExtraTaskOfferValue)
    {
        public static ICodec<BoardCounters> Codec { get; } = Codecs.Composite(
            Codecs.VarInt,
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
            static (taskLimit, remainingTasks, dailyTasksRefillTime, unknown0, unknown1, extraTaskOfferValue) => new BoardCounters(taskLimit, remainingTasks, dailyTasksRefillTime, unknown0, unknown1, extraTaskOfferValue)
        );
    }
}
