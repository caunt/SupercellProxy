using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>One featured decoration design and its voting identity.</summary>
public sealed record FeaturingDesignEntry
{
    /// <summary>Gets the candidate's avatar identity, when supplied.</summary>
    public LongIdentifier? AvatarIdentifier { get; init; }

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

    /// <summary>Decodes one featured design entry.</summary>
    public static FeaturingDesignEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        string farmName = stream.ReadString();
        ReadOnlyMemory<byte>? design = stream.ReadBoolean() ? stream.ReadByteArray() : null;
        int challengesComplete = stream.ReadVariableInt();
        int rnfLikes = stream.ReadVariableInt();
        int league = stream.ReadVariableInt();
        LongIdentifier? avatarIdentifier = stream.ReadBoolean() ? stream.ReadLongIdentifier() : null;

        return new FeaturingDesignEntry
        {
            FarmName = farmName,
            EncodedDesign = design,
            ChallengesComplete = challengesComplete,
            RnfLikes = rnfLikes,
            League = league,
            AvatarIdentifier = avatarIdentifier,
            FeaturingGroup = stream.ReadVariableInt(),
        };
    }

    /// <summary>Encodes one featured design entry.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteString(FarmName);
        stream.WriteBoolean(EncodedDesign is not null);

        if (EncodedDesign is { } design)
            stream.WriteByteArray(design.Span);

        stream.WriteVariableInt(ChallengesComplete);
        stream.WriteVariableInt(RnfLikes);
        stream.WriteVariableInt(League);
        stream.WriteBoolean(AvatarIdentifier is not null);

        if (AvatarIdentifier is { } avatarIdentifier)
            stream.WriteLongIdentifier(avatarIdentifier);

        stream.WriteVariableInt(FeaturingGroup);
    }
}
