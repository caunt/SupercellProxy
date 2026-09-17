using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// <summary>A decoded message-21945 entry with its nested records.</summary>
public sealed record Message21945Entry(
    LongIdentifier Identifier,
    int Value0,
    int Value1,
    int Value2,
    int Value3,
    bool Flag,
    string? Text,
    Message21945NestedEntry[]? Entries,
    LongIdentifier? OptionalIdentifier
)
{
    internal static Message21945Entry Decode(MessageStream stream)
    {
        LongIdentifier identifier = stream.ReadLongIdentifier();
        int value0 = stream.ReadVariableInt();
        int value1 = stream.ReadVariableInt();
        int value2 = stream.ReadVariableInt();
        int value3 = stream.ReadVariableInt();
        bool flag = stream.ReadBoolean();
        string? text = stream.ReadOptionalString();
        int count = stream.ReadVariableInt();

        if (count is < -1 or > 1000)
            throw new InvalidDataException(message: "Invalid message-21945 nested entry count.");

        Message21945NestedEntry[]? entries = count < 0 ? null : new Message21945NestedEntry[count];

        if (entries is not null)
        {
            for (int index = 0; index < entries.Length; index++)
                entries[index] = Message21945NestedEntry.Decode(stream);
        }

        return new(identifier, value0, value1, value2, value3, flag, text, entries, stream.ReadOptionalLongIdentifier());
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteLongIdentifier(Identifier);
        stream.WriteVariableInt(Value0);
        stream.WriteVariableInt(Value1);
        stream.WriteVariableInt(Value2);
        stream.WriteVariableInt(Value3);
        stream.WriteBoolean(Flag);
        stream.WriteOptionalString(Text);
        stream.WriteVariableInt(Entries?.Length ?? -1);

        if (Entries is not null)
        {
            foreach (Message21945NestedEntry entry in Entries)
                entry.Encode(stream);
        }

        stream.WriteOptionalLongIdentifier(OptionalIdentifier);
    }
}
