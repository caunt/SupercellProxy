using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Events;

/// <summary>One previous event neighborhood score entry.</summary>
/// <param name="Profile">The profile.</param>
/// <param name="Points">The points.</param>
public sealed record PreviousEventNeighborhoodScore(SupercellProxy.Networking.Protocol.Neighborhoods.NeighborhoodHeaderEntry Profile, int Points) : ICodecProvider<PreviousEventNeighborhoodScore>
{
    /// <inheritdoc />
    public static ICodec<PreviousEventNeighborhoodScore> Codec { get; } = Codecs.Composite(
        SupercellProxy.Networking.Protocol.Neighborhoods.NeighborhoodHeaderEntry.Codec,
        static value => value.Profile,
        Codecs.VarInt,
        static value => value.Points,
        static (profile, points) => new PreviousEventNeighborhoodScore(profile, points)
    );
}
