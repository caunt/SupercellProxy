using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Releases;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Voting;

/// <summary>One decoration design offered for a vote.</summary>
/// <param name="FarmName">The designer's farm name.</param>
/// <param name="EncodedDesign">The compressed canvas design, when present.</param>
/// <param name="ChallengesComplete">The number of completed design challenges.</param>
/// <param name="RnfLikes">The design's likes.</param>
/// <param name="League">The designer's league.</param>
/// <param name="AvatarId">The designer's avatar id, when present.</param>
/// <param name="FeaturingGroup">The featuring group of the design.</param>
public sealed record DecorationVoteCandidate(
    string FarmName,
    ReadOnlyMemory<byte>? EncodedDesign,
    int ChallengesComplete,
    int RnfLikes,
    int League,
    LongId? AvatarId,
    int FeaturingGroup
) : ICodecProvider<DecorationVoteCandidate>
{
    /// <inheritdoc />
    /// <remarks>1.73.81 writes the encoded design before the farm name.</remarks>
    public static ICodec<DecorationVoteCandidate> Codec { get; } = Codecs.Versioned(
        Release17381.Version,
        Codecs.Composite(
            Codecs.OptionalValue(Codecs.ByteArray),
            static candidate => candidate.EncodedDesign,
            Codecs.String,
            static candidate => candidate.FarmName,
            Codecs.VarInt,
            static candidate => candidate.ChallengesComplete,
            Codecs.VarInt,
            static candidate => candidate.RnfLikes,
            Codecs.VarInt,
            static candidate => candidate.League,
            Codecs.OptionalLongId,
            static candidate => candidate.AvatarId,
            Codecs.VarInt,
            static candidate => candidate.FeaturingGroup,
            static (encodedDesign, farmName, challengesComplete, rnfLikes, league, avatarId, featuringGroup) => new DecorationVoteCandidate(farmName, encodedDesign, challengesComplete, rnfLikes, league, avatarId, featuringGroup)
        ),
        Codecs.Composite(
            Codecs.String,
            static candidate => candidate.FarmName,
            Codecs.OptionalValue(Codecs.ByteArray),
            static candidate => candidate.EncodedDesign,
            Codecs.VarInt,
            static candidate => candidate.ChallengesComplete,
            Codecs.VarInt,
            static candidate => candidate.RnfLikes,
            Codecs.VarInt,
            static candidate => candidate.League,
            Codecs.OptionalLongId,
            static candidate => candidate.AvatarId,
            Codecs.VarInt,
            static candidate => candidate.FeaturingGroup,
            static (farmName, encodedDesign, challengesComplete, rnfLikes, league, avatarId, featuringGroup) => new DecorationVoteCandidate(farmName, encodedDesign, challengesComplete, rnfLikes, league, avatarId, featuringGroup)
        )
    );
}
