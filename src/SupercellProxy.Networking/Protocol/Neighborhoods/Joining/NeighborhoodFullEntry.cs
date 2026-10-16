using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>A neighbourhood with its profile and member list, as sent when the player opens or joins it.</summary>
/// <param name="Text">Free text of the neighbourhood, such as its motto.</param>
/// <param name="Header">Profile of the neighbourhood.</param>
/// <param name="Members">The neighbourhood's members; null when not sent.</param>
/// <param name="Values">Number list that is absent in the captures; its meaning is not identified.</param>
/// <param name="Flag">Flag that is clear in the captures; its meaning is not identified.</param>
public sealed record NeighborhoodFullEntry(
    string? Text,
    NeighborhoodHeaderEntry Header,
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
