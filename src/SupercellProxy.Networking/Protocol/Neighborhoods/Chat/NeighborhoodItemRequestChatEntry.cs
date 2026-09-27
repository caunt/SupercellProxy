using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>A neighborhood chat entry announcing one item request.</summary>
public sealed record NeighborhoodItemRequestChatEntry(
    int PostedTimestampSeconds,
    int Unknown0,
    bool FirstFlag,
    bool SecondFlag,
    LongIdentifier EntryIdentifier,
    LongIdentifier SenderHomeIdentifier,
    string SenderFarmName,
    int SenderLevel,
    int SenderRole,
    int Unknown1,
    int? ProfileEntryCount,
    int SecondaryTimestampSeconds,
    int ItemGlobalIdentifier,
    int Quantity,
    bool Unknown2,
    int? RelatedEntryCount
)
{
    internal static NeighborhoodItemRequestChatEntry Decode(MessageStream stream)
    {
        int posted = stream.ReadVariableInt();
        int unknown0 = stream.ReadVariableInt();
        bool firstFlag = stream.ReadBoolean();
        bool secondFlag = stream.ReadBoolean();
        LongIdentifier entryIdentifier = stream.ReadLongIdentifier();
        LongIdentifier home = stream.ReadLongIdentifier();
        string name = stream.ReadString();
        int level = stream.ReadVariableInt();
        int role = stream.ReadVariableInt();
        int unknown1 = stream.ReadVariableInt();
        int? profileCount = stream.ReadBoolean() ? stream.ReadVariableInt() : null;

        if (profileCount is < 0 or > 1024)
            throw new InvalidDataException(message: "The neighborhood chat profile count is invalid.");

        if (profileCount is > 0)
            throw new NotSupportedException(message: "Nonempty neighborhood chat profile entries have an unconfirmed layout.");

        int expiry = stream.ReadVariableInt();
        int item = stream.ReadVariableInt();
        int quantity = stream.ReadVariableInt();
        bool unknown2 = stream.ReadBoolean();
        int relatedCount = stream.ReadVariableInt();

        return relatedCount is < -1 or > 1000
            ? throw new InvalidDataException(message: "The neighborhood chat related-entry count is invalid.")
            : relatedCount > 0
            ? throw new NotSupportedException(message: "Nonempty neighborhood chat related entries have an unconfirmed layout.")
            : new NeighborhoodItemRequestChatEntry(
                posted,
                unknown0,
                firstFlag,
                secondFlag,
                entryIdentifier,
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

        stream.WriteVariableInt(PostedTimestampSeconds);
        stream.WriteVariableInt(Unknown0);
        stream.WriteBoolean(FirstFlag);
        stream.WriteBoolean(SecondFlag);
        stream.WriteLongIdentifier(EntryIdentifier);
        stream.WriteLongIdentifier(SenderHomeIdentifier);
        stream.WriteString(SenderFarmName);
        stream.WriteVariableInt(SenderLevel);
        stream.WriteVariableInt(SenderRole);
        stream.WriteVariableInt(Unknown1);
        stream.WriteBoolean(ProfileEntryCount is not null);

        if (ProfileEntryCount is { } profileCount)
            stream.WriteVariableInt(profileCount);

        stream.WriteVariableInt(SecondaryTimestampSeconds);
        stream.WriteVariableInt(ItemGlobalIdentifier);
        stream.WriteVariableInt(Quantity);
        stream.WriteBoolean(Unknown2);
        stream.WriteVariableInt(RelatedEntryCount ?? -1);
    }
}
