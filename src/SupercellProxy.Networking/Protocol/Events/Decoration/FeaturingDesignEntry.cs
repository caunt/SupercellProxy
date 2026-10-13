using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>One featured decoration design and its voting identity.</summary>
public sealed record FeaturingDesignEntry : ICodecProvider<FeaturingDesignEntry>
{
    /// <inheritdoc />
    public static ICodec<FeaturingDesignEntry> Codec { get; } = Codecs.Composite(
        Codecs.String,
        static value => value.FarmName,
        Codecs.OptionalValue(Codecs.ByteArray),
        static value => value.EncodedDesign,
        Codecs.VarInt,
        static value => value.ChallengesComplete,
        Codecs.VarInt,
        static value => value.RnfLikes,
        Codecs.VarInt,
        static value => value.League,
        Codecs.OptionalLongId,
        static value => value.AvatarId,
        Codecs.VarInt,
        static value => value.FeaturingGroup,
        static (farmName, design, challengesComplete, rnfLikes, league, avatarId, featuringGroup) => new FeaturingDesignEntry { FarmName = farmName, EncodedDesign = design, ChallengesComplete = challengesComplete, RnfLikes = rnfLikes, League = league, AvatarId = avatarId, FeaturingGroup = featuringGroup }
    );

    /// <summary>Gets the candidate's avatar identity, when supplied.</summary>
    public LongId? AvatarId { get; init; }

    /// <summary>Gets the completed challenge count.</summary>
    public int ChallengesComplete { get; init; }

    /// <summary>Gets the candidate's encoded canvas design, when supplied.</summary>
    public ReadOnlyMemory<byte>? EncodedDesign { get; init; }

    /// <summary>Gets the candidate's farm name.</summary>
    public string FarmName { get; init; } = string.Empty;

    /// <summary>Gets the candidate's featuring group.</summary>
    public int FeaturingGroup { get; init; }

    /// <summary>Gets the candidate's voting league.</summary>
    public int League { get; init; }

    /// <summary>Gets the saved RNF like count.</summary>
    public int RnfLikes { get; init; }
}
