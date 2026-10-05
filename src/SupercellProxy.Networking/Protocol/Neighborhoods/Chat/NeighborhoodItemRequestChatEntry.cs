using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>A neighborhood chat entry announcing one item request.</summary>
public sealed record NeighborhoodItemRequestChatEntry(
    int PostedTimestampSeconds,
    int Unknown0,
    bool FirstFlag,
    bool SecondFlag,
    LongId EntryId,
    LongId SenderHomeId,
    string SenderFarmName,
    int SenderLevel,
    int SenderRole,
    int Unknown1,
    int? ProfileEntryCount,
    int SecondaryTimestampSeconds,
    int ItemGlobalId,
    int Quantity,
    bool Unknown2,
    int? RelatedEntryCount
)
{
    internal static NeighborhoodItemRequestChatEntry Decode(MessageStream stream)
    {
        int posted = stream.ReadVarInt();
        int unknown0 = stream.ReadVarInt();
        bool firstFlag = stream.ReadBoolean();
        bool secondFlag = stream.ReadBoolean();
        LongId entryId = stream.ReadLongId();
        LongId home = stream.ReadLongId();
        string name = stream.ReadString();
        int level = stream.ReadVarInt();
        int role = stream.ReadVarInt();
        int unknown1 = stream.ReadVarInt();
        int? profileCount = stream.ReadBoolean() ? stream.ReadVarInt() : null;

        if (profileCount is < 0 or > 1024)
            throw new InvalidDataException(message: "The neighborhood chat profile count is invalid.");

        if (profileCount is > 0)
            throw new NotSupportedException(message: "Nonempty neighborhood chat profile entries have an unconfirmed layout.");

        int expiry = stream.ReadVarInt();
        int item = stream.ReadVarInt();
        int quantity = stream.ReadVarInt();
        bool unknown2 = stream.ReadBoolean();
        int relatedCount = stream.ReadVarInt();

        return relatedCount is < -1 or > 1000
            ? throw new InvalidDataException(message: "The neighborhood chat related-entry count is invalid.")
            : relatedCount > 0
            ? throw new NotSupportedException(message: "Nonempty neighborhood chat related entries have an unconfirmed layout.")
            : new NeighborhoodItemRequestChatEntry(
                posted,
                unknown0,
                firstFlag,
                secondFlag,
                entryId,
                home,
                name,
                level,
                role,
                unknown1,
                profileCount,
                expiry,
                item,
                quantity,
                unknown2,
                relatedCount < 0 ? null : relatedCount
            );
    }

    internal void Encode(MessageStream stream)
    {
        if (ProfileEntryCount is < 0 or > 1024 || RelatedEntryCount is < 0 or > 1000)
            throw new InvalidDataException(message: "The neighborhood chat entry count is invalid.");

        if (ProfileEntryCount is > 0 || RelatedEntryCount is > 0)
            throw new NotSupportedException(message: "Nonempty neighborhood chat entry lists have an unconfirmed layout.");

        stream.WriteVarInt(PostedTimestampSeconds);
        stream.WriteVarInt(Unknown0);
        stream.WriteBoolean(FirstFlag);
        stream.WriteBoolean(SecondFlag);
        stream.WriteLongId(EntryId);
        stream.WriteLongId(SenderHomeId);
        stream.WriteString(SenderFarmName);
        stream.WriteVarInt(SenderLevel);
        stream.WriteVarInt(SenderRole);
        stream.WriteVarInt(Unknown1);
        stream.WriteBoolean(ProfileEntryCount is not null);

        if (ProfileEntryCount is { } profileCount)
            stream.WriteVarInt(profileCount);

        stream.WriteVarInt(SecondaryTimestampSeconds);
        stream.WriteVarInt(ItemGlobalId);
        stream.WriteVarInt(Quantity);
        stream.WriteBoolean(Unknown2);
        stream.WriteVarInt(RelatedEntryCount ?? -1);
    }
}
