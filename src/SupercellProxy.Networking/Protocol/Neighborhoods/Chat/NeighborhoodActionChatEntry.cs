using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Retains the shared subtype 5, 9 and 10 stream-entry payload.</summary>
public sealed record NeighborhoodActionChatEntry(int EntryType, NeighborhoodChatEntryHeader Header, int Value, LongId[]? RelatedIds, bool Flag)
{
    internal static NeighborhoodActionChatEntry Decode(int entryType, MessageStream stream)
    {
        NeighborhoodChatEntryHeader header = NeighborhoodChatEntryHeader.Decode(stream);
        int value = stream.ReadVarInt();
        int count = stream.ReadVarInt();

        if (count is < -1 or > 1024)
            throw new InvalidDataException(message: "The neighborhood chat identifier count is invalid.");

        LongId[]? ids = count < 0 ? null : new LongId[count];

        if (ids is not null)
        {
            for (int index = 0; index < ids.Length; index++)
                ids[index] = stream.ReadLongId();
        }

        return new(entryType, header, value, ids, stream.ReadBoolean());
    }

    internal static bool Supports(int entryType)
    {
        return entryType is 5 or 9 or 10;
    }

    internal void Encode(MessageStream stream)
    {
        if (!Supports(EntryType))
            throw new InvalidDataException(message: "The neighborhood action entry type is invalid.");

        Header.Encode(stream);
        stream.WriteVarInt(Value);
        stream.WriteVarInt(RelatedIds?.Length ?? -1);

        foreach (LongId id in RelatedIds ?? [])
            stream.WriteLongId(id);

        stream.WriteBoolean(Flag);
    }
}
