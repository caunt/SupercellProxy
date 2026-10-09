using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Contains the common native neighborhood stream-entry fields.</summary>
public sealed record NeighborhoodChatEntryHeader(
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
    NeighborhoodChatProfileEntry[]? ProfileEntries
)
{
    internal static NeighborhoodChatEntryHeader Decode(MessageStream stream)
    {
        int posted = stream.ReadVarInt();
        int unknown0 = stream.ReadVarInt();
        bool first = stream.ReadBoolean();
        bool second = stream.ReadBoolean();
        LongId entry = stream.ReadLongId();
        LongId sender = stream.ReadLongId();
        string name = stream.ReadString();
        int level = stream.ReadVarInt();
        int role = stream.ReadVarInt();
        int unknown1 = stream.ReadVarInt();
        NeighborhoodChatProfileEntry[]? profiles = null;

        if (stream.ReadBoolean())
        {
            int count = stream.ReadVarInt();

            if (count is < 0 or > 1024)
                throw new InvalidDataException(message: "The neighborhood chat profile count is invalid.");

            profiles = new NeighborhoodChatProfileEntry[count];

            for (int index = 0; index < count; index++)
                profiles[index] = NeighborhoodChatProfileEntry.Decode(stream);
        }

        return new(posted, unknown0, first, second, entry, sender, name, level, role, unknown1, profiles);
    }

    internal void Encode(MessageStream stream)
    {
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
        stream.WriteBoolean(ProfileEntries is not null);

        if (ProfileEntries is not { } profiles) return;

        stream.WriteVarInt(profiles.Length);

        foreach (NeighborhoodChatProfileEntry profile in profiles)
            profile.Encode(stream);
    }
}
