using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Events;

/// <summary>One neighbourhood's final standing in the previous neighbourhood event.</summary>
/// <param name="Profile">The neighbourhood's public profile: name, badge, members and the like.</param>
/// <param name="Points">Points the neighbourhood finished with.</param>
public sealed record PreviousEventNeighborhoodScore(NeighborhoodHeaderEntry Profile, int Points) : ICodecProvider<PreviousEventNeighborhoodScore>
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
