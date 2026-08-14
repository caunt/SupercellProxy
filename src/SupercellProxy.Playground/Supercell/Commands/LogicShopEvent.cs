using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// One shop event in server command 355.
/// </summary>
public sealed record LogicShopEvent
{
    public Memory<byte>? BinaryData { get; init; }
    public string TextData { get; init; } = string.Empty;
    public int EventId { get; init; }
    public int Unknown0 { get; init; }
    public string UnknownString0 { get; init; } = string.Empty;
    public int EventType { get; init; }
    public int Unknown1 { get; init; }
    public int Unknown2 { get; init; }
    public int Unknown3 { get; init; }
    public int Unknown4 { get; init; }
    public int Unknown5 { get; init; }
    public int Unknown6 { get; init; }
    public int Unknown7 { get; init; }
    public int Unknown8 { get; init; }
    public int Unknown9 { get; init; }
    public int Unknown10 { get; init; }
    public string UnknownString1 { get; init; } = string.Empty;

    internal static LogicShopEvent Decode(SupercellStream stream)
    {
        var usesBinaryData = stream.ReadBoolean();
        var binaryData = usesBinaryData ? stream.ReadVarIntByteArray() : null;
        var textData = usesBinaryData ? string.Empty : stream.ReadString();

        return new LogicShopEvent
        {
            BinaryData = binaryData,
            TextData = textData,
            EventId = stream.ReadVarInt(),
            Unknown0 = stream.ReadVarInt(),
            UnknownString0 = stream.ReadString(),
            EventType = stream.ReadVarInt(),
            Unknown1 = stream.ReadVarInt(),
            Unknown2 = stream.ReadVarInt(),
            Unknown3 = stream.ReadVarInt(),
            Unknown4 = stream.ReadVarInt(),
            Unknown5 = stream.ReadVarInt(),
            Unknown6 = stream.ReadVarInt(),
            Unknown7 = stream.ReadVarInt(),
            Unknown8 = stream.ReadVarInt(),
            Unknown9 = stream.ReadVarInt(),
            Unknown10 = stream.ReadVarInt(),
            UnknownString1 = stream.ReadString()
        };
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(BinaryData is not null);

        if (BinaryData is not null)
            stream.WriteVarIntByteArray(BinaryData.Value.Span);
        else
            stream.WriteString(TextData);

        stream.WriteVarInt(EventId);
        stream.WriteVarInt(Unknown0);
        stream.WriteString(UnknownString0);
        stream.WriteVarInt(EventType);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        stream.WriteVarInt(Unknown5);
        stream.WriteVarInt(Unknown6);
        stream.WriteVarInt(Unknown7);
        stream.WriteVarInt(Unknown8);
        stream.WriteVarInt(Unknown9);
        stream.WriteVarInt(Unknown10);
        stream.WriteString(UnknownString1);
    }
}
