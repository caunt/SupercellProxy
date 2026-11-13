using System.Text;

using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// <summary>Describes one farm entry carried by friend, follower, and following messages.</summary>
/// <param name="HomeId">Home id of the farm the entry describes; relationship updates match entries by this id.</param>
/// <param name="Name">The farm's display name, or <see langword="null"/> when the server sends none; text is UTF-8 and at most 900,000 bytes.</param>
/// <param name="FacebookId">The farm owner's Facebook id, or <see langword="null"/> when the account is not linked to Facebook.</param>
/// <param name="GameCenterId">The farm owner's Game Center id, or <see langword="null"/> when the account is not linked to Game Center.</param>
/// <param name="ProfilePictureAddress">Address of the farm owner's profile picture, or <see langword="null"/> when the entry has none.</param>
/// <param name="ExperienceLevel">The farm owner's experience level.</param>
/// <param name="SortValue">The variable-length integer written right after the level; its meaning is not yet identified.</param>
/// <param name="RelationshipStatus">
/// The native relationship code: 2 for an incoming friend request, 3 for an outgoing one, 4 for a friend, 6 blocked, 7 hidden, 8 removed,
/// 9 unblocked and 10 unhidden; codes 1, 5 and 11 are not yet identified.
/// </param>
/// <param name="BlockTimestamp">The last variable-length integer of the entry, named for when the farm was blocked; its unit and use are not yet identified.</param>
public sealed record FriendEntry(LongId HomeId,
    string? Name,
    string? FacebookId,
    string? GameCenterId,
    string? ProfilePictureAddress,
    int ExperienceLevel,
    int SortValue,
    int RelationshipStatus,
    int BlockTimestamp) : ICodecProvider<FriendEntry>
{
    internal const int MinimumEncodedSize = 28;

    private const int MaximumStringByteLength = 900_000;

    /// <inheritdoc />
    public static ICodec<FriendEntry> Codec { get; } = Codecs.Composite(Codecs.LongId,
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
        static (homeId, name, facebookId, gameCenterId, profilePictureAddress, experienceLevel, sortValue, relationshipStatus, blockTimestamp) => new FriendEntry(homeId, name, facebookId, gameCenterId, profilePictureAddress, experienceLevel, sortValue, relationshipStatus, blockTimestamp));

    /// <summary>Omits private farm and social-provider data from diagnostics.</summary>
    /// <returns>Always the type name <c>FriendEntry</c>, whatever the entry holds.</returns>
    public override string ToString()
    {
        return nameof(FriendEntry);
    }

    private static string? ReadText(MessageChannel stream)
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
