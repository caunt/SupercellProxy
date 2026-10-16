using System.Text;

using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// <summary>Describes one farm entry carried by friend, follower, and following messages.</summary>
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
) : ICodecProvider<FriendEntry>
{
    internal const int MinimumEncodedSize = 28;

    private const int MaximumStringByteLength = 900_000;
    /// <inheritdoc />
    public static ICodec<FriendEntry> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static entry => entry.HomeId,
        TextCodec(),
        static entry => entry.Name,
        TextCodec(),
        static entry => entry.FacebookId,
        TextCodec(),
        static entry => entry.GameCenterId,
        TextCodec(),
        static entry => entry.ProfilePictureAddress,
        Codecs.VarInt,
        static entry => entry.ExperienceLevel,
        Codecs.VarInt,
        static entry => entry.SortValue,
        Codecs.VarInt,
        static entry => entry.RelationshipStatus,
        Codecs.VarInt,
        static entry => entry.BlockTimestamp,
        static (homeId, name, facebookId, gameCenterId, profilePictureAddress, experienceLevel, sortValue, relationshipStatus, blockTimestamp) => new FriendEntry(homeId, name, facebookId, gameCenterId, profilePictureAddress, experienceLevel, sortValue, relationshipStatus, blockTimestamp)
    );

    /// <summary>Omits private farm and social-provider data from diagnostics.</summary>
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

    private static ICodec<string?> TextCodec()
    {
        return Codecs.Create(ReadText, WriteText);
    }

    private static void WriteText(ProtocolEncoder encoder, string? value)
    {
        if (value is not null && Encoding.UTF8.GetByteCount(value) > MaximumStringByteLength)
            throw new InvalidDataException(message: "Friend-entry text exceeds the native byte limit.");

        encoder.WriteOptionalString(value);
    }
}
