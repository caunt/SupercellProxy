using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// <summary>The fixed fields of a message-21945 nested record.</summary>
public sealed record Message21945NestedEntry(
    LongIdentifier Identifier,
    string? Text,
    int Value0,
    int Value1,
    int Value2,
    int Value3,
    int Value4,
    int Value5,
    int Value6,
    int Value7
)
{
    internal static Message21945NestedEntry Decode(MessageStream stream)
    {
        return new(
            stream.ReadLongIdentifier(),
            stream.ReadOptionalString(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt()
        );
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteLongIdentifier(Identifier);
        stream.WriteOptionalString(Text);
        stream.WriteVariableInt(Value0);
        stream.WriteVariableInt(Value1);
        stream.WriteVariableInt(Value2);
        stream.WriteVariableInt(Value3);
        stream.WriteVariableInt(Value4);
        stream.WriteVariableInt(Value5);
        stream.WriteVariableInt(Value6);
        stream.WriteVariableInt(Value7);
    }
}
