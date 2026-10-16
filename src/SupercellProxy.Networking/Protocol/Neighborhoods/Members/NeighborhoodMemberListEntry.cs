using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>One member list kept for the player, with its owner and members; what kind of list it is is not identified.</summary>
/// <param name="Id">Id of the list; it equals the owner's account id in the captures.</param>
/// <param name="Value0">Global id of a table entry, 2200002 in the captures; what it names is not identified.</param>
/// <param name="Value1">Number that is 3 in the captures; its meaning is not identified.</param>
/// <param name="Value2">Number that is 3 in the captures; its meaning is not identified.</param>
/// <param name="Value3">Unix time in seconds, likely when the list was made; not confirmed.</param>
/// <param name="Flag">Flag that is clear in the captures; its meaning is not identified.</param>
/// <param name="Name">Text identifier of the list, a GUID in the captures.</param>
/// <param name="Members">The members, or null when native writes a negative count.</param>
/// <param name="OwnerId">Account that owns the list.</param>
public sealed record NeighborhoodMemberListEntry(
    LongId Id,
    int Value0,
    int Value1,
    int Value2,
    int Value3,
    bool Flag,
    string? Name,
    ReadOnlyMemory<NeighborhoodMemberSummary>? Members,
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
