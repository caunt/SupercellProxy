using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Contains the complete native derby task-board response.</summary>
public sealed record DerbyBoardMessage : IMessage
{
    /// <summary>Gets the player's active task-board index, or minus one.</summary>
    public int ActiveTaskBoardIndex { get; init; }
    /// <summary>Gets the optional auxiliary board entries.</summary>
    public DerbyBoardAuxiliaryEntry[]? AuxiliaryEntries { get; init; }
    /// <summary>Gets the cached current derby race instance.</summary>
    public LongId? CurrentInstanceId { get; init; }
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

    /// <summary>Decodes every board field in native order.</summary>
    public static DerbyBoardMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        DerbyBoardMessage message = new()
        {
            TaskLimit = stream.ReadVarInt(),
            RemainingTasks = stream.ReadVarInt(),
            ActiveTaskBoardIndex = stream.ReadVarInt(),
            Unknown0 = stream.ReadVarInt(),
            Unknown1 = stream.ReadVarInt(),
            ExtraTaskOfferValue = stream.ReadVarInt(),
            CurrentInstanceId = stream.ReadOptionalLongId(),
            Flag0 = stream.ReadBoolean(),
            Flag1 = stream.ReadBoolean(),
            Flag2 = stream.ReadBoolean(),
            PreviousInstanceId = stream.ReadOptionalLongId(),
            Tasks = DerbyMessageCodec.ReadOptionalArray(stream, DerbyTaskEntry.Decode),
            AuxiliaryEntries = DerbyMessageCodec.ReadOptionalArray(stream, DerbyBoardAuxiliaryEntry.Decode),
            Unknown2 = stream.ReadVarInt(),
            Unknown3 = stream.ReadVarInt(),
            Unknown4 = stream.ReadVarInt(),
            Unknown5 = stream.ReadVarInt(),
            Flag3 = stream.ReadBoolean(),
            Unknown6 = stream.ReadVarInt(),
            Groups = stream.ReadArray(DerbyBoardGroupEntry.Decode),
            Supplement = stream.ReadBoolean() ? DerbyBoardSupplement.Decode(stream) : null,
        };

        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes every board field, preserving absent collections.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(TaskLimit);
        stream.WriteVarInt(RemainingTasks);
        stream.WriteVarInt(ActiveTaskBoardIndex);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(ExtraTaskOfferValue);
        stream.WriteOptionalLongId(CurrentInstanceId);
        stream.WriteBoolean(Flag0);
        stream.WriteBoolean(Flag1);
        stream.WriteBoolean(Flag2);
        stream.WriteOptionalLongId(PreviousInstanceId);
        DerbyMessageCodec.WriteOptionalArray(stream, Tasks, static (writer, entry) => entry.Encode(writer));
        DerbyMessageCodec.WriteOptionalArray(stream, AuxiliaryEntries, static (writer, entry) => entry.Encode(writer));
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        stream.WriteVarInt(Unknown5);
        stream.WriteBoolean(Flag3);
        stream.WriteVarInt(Unknown6);
        stream.WriteArray<DerbyBoardGroupEntry>(Groups, static (writer, entry) => entry.Encode(writer));
        stream.WriteBoolean(Supplement is not null);
        Supplement?.Encode(stream);
    }

    /// <summary>Omits identifiers and player details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyBoardMessage);
    }
}
