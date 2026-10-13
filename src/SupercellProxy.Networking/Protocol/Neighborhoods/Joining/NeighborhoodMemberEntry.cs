using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>One neighborhood member entry entry.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="Name">The name.</param>
/// <param name="ExpLevel">The exp level.</param>
/// <param name="Role">The role.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
/// <param name="Value2">The value2.</param>
/// <param name="Flag0">The flag0.</param>
/// <param name="Flag1">The flag1.</param>
/// <param name="Flag2">The flag2.</param>
/// <param name="Flag3">The flag3.</param>
/// <param name="Value3">The value3.</param>
/// <param name="Value4">The value4.</param>
/// <param name="Value5">The value5.</param>
/// <param name="Timestamp0">The timestamp0.</param>
/// <param name="Timestamp1">The timestamp1.</param>
/// <param name="HomeId">The home id.</param>
/// <param name="NeighborhoodId">The neighborhood id.</param>
public sealed record NeighborhoodMemberEntry(
    LongId AvatarId,
    string? Name,
    int ExpLevel,
    int Role,
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
        static entry => entry.ExpLevel,
        Codecs.VarInt,
        static entry => entry.Role,
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
            expLevel,
            role,
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
            expLevel,
            role,
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
