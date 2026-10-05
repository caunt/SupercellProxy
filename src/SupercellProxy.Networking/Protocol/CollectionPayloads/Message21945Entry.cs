using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// <summary>A decoded message-21945 entry with its nested records.</summary>
public sealed record Message21945Entry(
    LongId Id,
    int Value0,
    int Value1,
    int Value2,
    int Value3,
    bool Flag,
    string? Text,
    Message21945NestedEntry[]? Entries,
    LongId? OptionalId
)
{
    internal static Message21945Entry Decode(MessageStream stream)
    {
        LongId id = stream.ReadLongId();
        int value0 = stream.ReadVarInt();
        int value1 = stream.ReadVarInt();
        int value2 = stream.ReadVarInt();
        int value3 = stream.ReadVarInt();
        bool flag = stream.ReadBoolean();
        string? text = stream.ReadOptionalString();
        int count = stream.ReadVarInt();

        if (count is < -1 or > 1000)
            throw new InvalidDataException(message: "Invalid message-21945 nested entry count.");

        Message21945NestedEntry[]? entries = count < 0 ? null : new Message21945NestedEntry[count];

        if (entries is not null)
        {
            for (int index = 0; index < entries.Length; index++)
                entries[index] = Message21945NestedEntry.Decode(stream);
        }

        return new(id, value0, value1, value2, value3, flag, text, entries, stream.ReadOptionalLongId());
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteLongId(Id);
        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
        stream.WriteVarInt(Value2);
        stream.WriteVarInt(Value3);
        stream.WriteBoolean(Flag);
        stream.WriteOptionalString(Text);
        stream.WriteVarInt(Entries?.Length ?? -1);

        if (Entries is not null)
        {
            foreach (Message21945NestedEntry entry in Entries)
                entry.Encode(stream);
        }

        stream.WriteOptionalLongId(OptionalId);
    }
}
