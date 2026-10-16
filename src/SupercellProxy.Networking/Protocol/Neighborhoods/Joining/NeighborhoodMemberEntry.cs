using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>One member of a neighbourhood as its member list shows them. Meanings come from captured lists.</summary>
/// <param name="AvatarId">Account of the member.</param>
/// <param name="Name">Farm name of the member.</param>
/// <param name="Role">
/// The member's neighbourhood role, 1 to 4 in the captures; it matches the role the member's own avatar carries in its
/// neighbourhood data.
/// </param>
/// <param name="ExpLevel">The member's farm level; it rises with the member's level-ups in the captures.</param>
/// <param name="Value0">Per-member number, 7 to 18 in the captures; its meaning is not identified.</param>
/// <param name="Value1">Position of the member in the list, starting at 1.</param>
/// <param name="Value2">Number that is 0 in every capture; its meaning is not identified.</param>
/// <param name="Flag0">Flag that is clear in every capture; its meaning is not identified.</param>
/// <param name="Flag1">Flag that is set in every capture; its meaning is not identified.</param>
/// <param name="Flag2">Per-member flag; its meaning is not identified.</param>
/// <param name="Flag3">Per-member flag that matches <paramref name="Flag2"/> in the captures; its meaning is not identified.</param>
/// <param name="Value3">Number that is 0 in every capture; its meaning is not identified.</param>
/// <param name="Value4">Unix time in seconds, always equal to <paramref name="Timestamp0"/> in the captures.</param>
/// <param name="Value5">Per-member number that changes between captures; its meaning is not identified.</param>
/// <param name="Timestamp0">Recent Unix time in seconds, likely the member's last activity; not confirmed.</param>
/// <param name="Timestamp1">Older Unix time in seconds, likely when the member joined; not confirmed.</param>
/// <param name="HomeId">Optional id that two members share in the captures; its meaning is not identified.</param>
/// <param name="NeighborhoodId">Optional id that differs between members in the captures; its meaning is not identified.</param>
public sealed record NeighborhoodMemberEntry(
    LongId AvatarId,
    string? Name,
    int Role,
    int ExpLevel,
    int Value0,
    int Value1,
    int Value2,
    bool Flag0,
    bool Flag1,
    bool Flag2,
    bool Flag3,
    int Value3,
    int Value4,
    int Value5,
    long Timestamp0,
    long Timestamp1,
    LongId? HomeId,
    LongId? NeighborhoodId
) : ICodecProvider<NeighborhoodMemberEntry>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodMemberEntry> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static entry => entry.AvatarId,
        Codecs.OptionalString,
        static entry => entry.Name,
        Codecs.VarInt,
        static entry => entry.Role,
        Codecs.VarInt,
        static entry => entry.ExpLevel,
        Codecs.VarInt,
        static entry => entry.Value0,
        Codecs.VarInt,
        static entry => entry.Value1,
        Codecs.VarInt,
        static entry => entry.Value2,
        MemberFlags.Codec,
        static entry => new MemberFlags(entry.Flag0, entry.Flag1, entry.Flag2, entry.Flag3),
        Codecs.VarInt,
        static entry => entry.Value3,
        Codecs.VarInt,
        static entry => entry.Value4,
        Codecs.Int32,
        static entry => entry.Value5,
        Codecs.Int64,
        static entry => entry.Timestamp0,
        Codecs.Int64,
        static entry => entry.Timestamp1,
        Codecs.OptionalLongId,
        static entry => entry.HomeId,
        Codecs.OptionalLongId,
        static entry => entry.NeighborhoodId,
        static (
            avatarId,
            name,
            role,
            expLevel,
            value0,
            value1,
            value2,
            memberFlags,
            value3,
            value4,
            value5,
            timestamp0,
            timestamp1,
            homeId,
            neighborhoodId
        ) => new NeighborhoodMemberEntry(
            avatarId,
            name,
            role,
            expLevel,
            value0,
            value1,
            value2,
            memberFlags.Flag0,
            memberFlags.Flag1,
            memberFlags.Flag2,
            memberFlags.Flag3,
            value3,
            value4,
            value5,
            timestamp0,
            timestamp1,
            homeId,
            neighborhoodId
        )
    );

    // The four member flags native reads together.
    private readonly record struct MemberFlags(bool Flag0, bool Flag1, bool Flag2, bool Flag3)
    {
        public static ICodec<MemberFlags> Codec { get; } = Codecs.Composite(
            Codecs.Boolean,
            static segment => segment.Flag0,
            Codecs.Boolean,
            static segment => segment.Flag1,
            Codecs.Boolean,
            static segment => segment.Flag2,
            Codecs.Boolean,
            static segment => segment.Flag3,
            static (flag0, flag1, flag2, flag3) => new MemberFlags(flag0, flag1, flag2, flag3)
        );
    }
}
