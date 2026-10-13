using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>The neighborhood full entry.</summary>
/// <param name="Text">The text.</param>
/// <param name="Header">The header.</param>
/// <param name="Members">The members.</param>
/// <param name="Values">The values.</param>
/// <param name="Flag">The flag.</param>
public sealed record NeighborhoodFullEntry(
    string? Text,
    SupercellProxy.Networking.Protocol.Neighborhoods.NeighborhoodHeaderEntry Header,
    ReadOnlyMemory<NeighborhoodMemberEntry>? Members,
    ReadOnlyMemory<int>? Values,
    bool Flag
) : ICodecProvider<NeighborhoodFullEntry>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodFullEntry> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static entry => entry.Text,
        NeighborhoodHeaderEntry.Codec,
        static entry => entry.Header,
        Codecs.NullableMemory(NeighborhoodMemberEntry.Codec),
        static entry => entry.Members,
        Codecs.NullableMemory(Codecs.VarInt),
        static entry => entry.Values,
        Codecs.Boolean,
        static entry => entry.Flag,
        static (text, header, members, values, flag) => new NeighborhoodFullEntry(text, header, members, values, flag)
    );
}
