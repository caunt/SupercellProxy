using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>A neighbourhood profile paired with a number, as ranking lists send them.</summary>
/// <param name="Profile">Profile of the neighbourhood.</param>
/// <param name="Value">Score of the neighbourhood in the list; ranking lists are sorted by it in the captures.</param>
public sealed record NeighborhoodHeaderEntryValue(NeighborhoodHeaderEntry Profile, int Value) : ICodecProvider<NeighborhoodHeaderEntryValue>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodHeaderEntryValue> Codec { get; } = Codecs.Composite(
        NeighborhoodHeaderEntry.Codec,
        static entry => entry.Profile,
        Codecs.VarInt,
        static entry => entry.Value,
        static (profile, value) => new NeighborhoodHeaderEntryValue(profile, value)
    );
}
