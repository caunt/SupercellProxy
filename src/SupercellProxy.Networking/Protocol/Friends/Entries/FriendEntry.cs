using System.Text;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// Describes one farm entry carried by friend, follower, and following messages.
public sealed record FriendEntry(
    LongId HomeId,
    string? Name,
    string? FacebookId,
    string? GameCenterId,
    string? ProfilePictureAddress,
    int ExperienceLevel,
    int SortValue,
    int RelationshipStatus,
    int BlockTimestamp
)
{
    internal const int MinimumEncodedSize = 28;
    private const int MaximumStringByteLength = 900_000;

    /// Decodes every field in native wire order without interpreting unknown relationship-status values.
    public static FriendEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new FriendEntry(
            stream.ReadLongId(),
            ReadText(stream),
            ReadText(stream),
            ReadText(stream),
            ReadText(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    /// Encodes this entry in native wire order.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(HomeId);
        WriteText(stream, Name);
        WriteText(stream, FacebookId);
        WriteText(stream, GameCenterId);
        WriteText(stream, ProfilePictureAddress);
        stream.WriteVarInt(ExperienceLevel);
        stream.WriteVarInt(SortValue);
        stream.WriteVarInt(RelationshipStatus);
        stream.WriteVarInt(BlockTimestamp);
    }

    /// Omits private farm and social-provider data from diagnostics.
    public override string ToString()
    {
        return nameof(FriendEntry);
    }

    private static string? ReadText(MessageStream stream)
    {
        int length = stream.ReadInt32();

        return length < 0
            ? null
            : length > MaximumStringByteLength
            ? throw new InvalidDataException(message: "Friend-entry text exceeds the native byte limit.")
            : Encoding.UTF8.GetString(stream.ReadBytes(length));
    }

    private static void WriteText(MessageStream stream, string? value)
    {
        if (value is not null && Encoding.UTF8.GetByteCount(value) > MaximumStringByteLength)
            throw new InvalidDataException(message: "Friend-entry text exceeds the native byte limit.");

        stream.WriteOptionalString(value);
    }
}
