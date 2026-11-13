using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>One featured decoration design and its voting identity.</summary>
public sealed record FeaturingDesignEntry : ICodecProvider<FeaturingDesignEntry>
{
    /// <inheritdoc />
    public static ICodec<FeaturingDesignEntry> Codec { get; } = Codecs.Composite(Codecs.Utf8String,
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
        static (farmName, design, challengesComplete, rnfLikes, league, avatarId, featuringGroup) => new FeaturingDesignEntry
        {
            FarmName = farmName,
            EncodedDesign = design,
            ChallengesComplete = challengesComplete,
            RnfLikes = rnfLikes,
            League = league,
            AvatarId = avatarId,
            FeaturingGroup = featuringGroup
        });

    /// <summary>Gets or sets the candidate's avatar identity, when supplied.</summary>
    /// <value>The designer's avatar id, written as an optional long id after the league; <see langword="null"/> when the server leaves it out.</value>
    public LongId? AvatarId { get; init; }

    /// <summary>Gets or sets the completed challenge count.</summary>
    /// <value>How many of the event's design challenges the designer completed, sent as a variable-length integer.</value>
    public int ChallengesComplete { get; init; }

    /// <summary>Gets or sets the candidate's encoded canvas design, when supplied.</summary>
    /// <value>The compressed canvas design document that decodes into a <see cref="DecorationCanvasDesignSnapshot"/>, or <see langword="null"/> when the entry carries no design.</value>
    public ReadOnlyMemory<byte>? EncodedDesign { get; init; }

    /// <summary>Gets or sets the candidate's farm name.</summary>
    /// <value>The designer's farm name, the first field of the entry on the wire; empty when not supplied.</value>
    public string FarmName { get; init; } = string.Empty;

    /// <summary>Gets or sets the candidate's featuring group.</summary>
    /// <value>The featuring group number the server assigned to the design, the last field of the entry.</value>
    public int FeaturingGroup { get; init; }

    /// <summary>Gets or sets the candidate's voting league.</summary>
    /// <value>The league number reported for the designer; its range is not yet identified.</value>
    public int League { get; init; }

    /// <summary>Gets or sets the saved RNF like count.</summary>
    /// <value>The number of likes the server reports for the design; what the RNF prefix stands for is not yet identified.</value>
    public int RnfLikes { get; init; }
}
