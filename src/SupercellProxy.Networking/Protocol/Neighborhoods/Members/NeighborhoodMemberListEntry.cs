using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>One neighborhood member list entry entry.</summary>
/// <param name="Id">The id.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
/// <param name="Value2">The value2.</param>
/// <param name="Value3">The value3.</param>
/// <param name="Flag">The flag.</param>
/// <param name="Name">The name.</param>
/// <param name="Members">The members, or null when native writes a negative count.</param>
/// <param name="OwnerId">The owner id.</param>
public sealed record NeighborhoodMemberListEntry(
    LongId Id,
    int Value0,
    int Value1,
    int Value2,
    int Value3,
    bool Flag,
    string? Name,
    ReadOnlyMemory<SupercellProxy.Networking.Protocol.Neighborhoods.Members.NeighborhoodMemberSummary>? Members,
    LongId? OwnerId
) : ICodecProvider<NeighborhoodMemberListEntry>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodMemberListEntry> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static entry => entry.Id,
        Codecs.VarInt,
        static entry => entry.Value0,
        Codecs.VarInt,
        static entry => entry.Value1,
        Codecs.VarInt,
        static entry => entry.Value2,
        Codecs.VarInt,
        static entry => entry.Value3,
        Codecs.Boolean,
        static entry => entry.Flag,
        Codecs.OptionalString,
        static entry => entry.Name,
        Codecs.NullableMemory(NeighborhoodMemberSummary.Codec),
        static entry => entry.Members,
        Codecs.OptionalLongId,
        static entry => entry.OwnerId,
        static (id, value0, value1, value2, value3, flag, name, members, ownerId) => new NeighborhoodMemberListEntry(id, value0, value1, value2, value3, flag, name, members, ownerId)
    );
}
