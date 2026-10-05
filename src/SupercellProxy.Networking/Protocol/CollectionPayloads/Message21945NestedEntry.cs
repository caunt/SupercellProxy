using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// <summary>The fixed fields of a message-21945 nested record.</summary>
public sealed record Message21945NestedEntry(LongId Id, string? Text, int Value0, int Value1, int Value2, int Value3, int Value4, int Value5, int Value6, int Value7)
{
    internal static Message21945NestedEntry Decode(MessageStream stream)
    {
        return new(
            stream.ReadLongId(),
            stream.ReadOptionalString(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteLongId(Id);
        stream.WriteOptionalString(Text);
        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
        stream.WriteVarInt(Value2);
        stream.WriteVarInt(Value3);
        stream.WriteVarInt(Value4);
        stream.WriteVarInt(Value5);
        stream.WriteVarInt(Value6);
        stream.WriteVarInt(Value7);
    }
}
