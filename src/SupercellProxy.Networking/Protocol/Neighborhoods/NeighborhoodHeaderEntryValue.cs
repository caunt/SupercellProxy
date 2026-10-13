using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood Profile Value contract.
/// </summary>
/// <summary>
/// Defines the Profile contract.
/// </summary>
/// <summary>
/// Defines the Value contract.
/// </summary>
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
