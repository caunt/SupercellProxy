using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// Native map-game pawn structure encoded by the shared 1.72.84 helper at 0x10065c78c.
/// Semantic names for the stripped fields are not yet proven.
/// </summary>
public sealed record MapGamePawn
{
    /// <summary>
    /// Initializes a new <see cref="MapGamePawn"/> instance.
    /// </summary>
    public MapGamePawn(
        LongIdentifier? avatarIdentifier,
        LongIdentifier? neighborhoodIdentifier,
        int experienceLevel,
        int currentNodeIdentifier,
        int emptyNodesTravelled,
        int sanctuaryAnimalEscapeAllowance,
        int sanctuaryAnimalEscapeProgress,
        ReadOnlyMemory<int> visitedNodeIdentifiers,
        ReadOnlyMemory<int> selectedOptions,
        string? name,
        MapGamePawnEmblem? emblem,
        int unknownGlobalIdentifier,
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> notifications
    )
    {
        AvatarIdentifier = avatarIdentifier;
        NeighborhoodIdentifier = neighborhoodIdentifier;
        ExperienceLevel = experienceLevel;
        CurrentNodeIdentifier = currentNodeIdentifier;
        EmptyNodesTravelled = emptyNodesTravelled;
        SanctuaryAnimalEscapeAllowance = sanctuaryAnimalEscapeAllowance;
        SanctuaryAnimalEscapeProgress = sanctuaryAnimalEscapeProgress;
        VisitedNodeIdentifiers = visitedNodeIdentifiers.ToArray();
        SelectedOptions = selectedOptions.ToArray();
        Name = name;
        Emblem = emblem;
        UnknownGlobalIdentifier = unknownGlobalIdentifier;
        Notifications = notifications.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId0</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId0")]
    public LongIdentifier? AvatarIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">CurrentNodeIdentifier</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown1")]
    public int CurrentNodeIdentifier { get; }

    /// <summary>
    /// Gets the participant's emblem.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownNestedData")]
    public MapGamePawnEmblem? Emblem { get; }

    /// <summary>
    /// Gets the number of empty nodes travelled.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown2")]
    public int EmptyNodesTravelled { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExperienceLevel</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown0")]
    public int ExperienceLevel { get; }

    /// <summary>
    /// Gets the participant's name.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownString")]
    public string? Name { get; }

    /// <summary>
    /// Gets the participant's neighborhood identifier.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId1")]
    public LongIdentifier? NeighborhoodIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">Notifications</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownPairs")]
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair> Notifications { get; init; }

    /// <summary>
    /// Gets the current empty-node allowance before a carried sanctuary animal can escape.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown3")]
    public int SanctuaryAnimalEscapeAllowance { get; }

    /// <summary>
    /// Gets the retained sanctuary-animal escape progress counter.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown4")]
    public int SanctuaryAnimalEscapeProgress { get; }

    /// <summary>
    /// Gets the selected map-game profile options.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalIds")]
    public ReadOnlyMemory<int> SelectedOptions { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId")]
    public int UnknownGlobalIdentifier { get; }

    /// <summary>
    /// Gets the ordered, unique node identifiers revealed by this pawn's movements.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownValues")]
    public ReadOnlyMemory<int> VisitedNodeIdentifiers { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGamePawn Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier? avatarIdentifier = MapGameFieldCodec.ReadOptionalLongIdentifier(stream);
        LongIdentifier? neighborhoodIdentifier = MapGameFieldCodec.ReadOptionalLongIdentifier(stream);
        int experienceLevel = stream.ReadVariableInt();
        int currentNodeIdentifier = stream.ReadVariableInt();
        int emptyNodesTravelled = stream.ReadVariableInt();
        int sanctuaryAnimalEscapeAllowance = stream.ReadVariableInt();
        int sanctuaryAnimalEscapeProgress = stream.ReadVariableInt();
        int[] visitedNodeIdentifiers = CommandVariableIntArrayField.DecodeValues(stream.ReadVariableInt(), stream);
        ReadOnlyMemory<int> selectedOptions = CommandDataReferenceArrayField.Decode(stream).GlobalIdentifiers;
        string? name = stream.ReadBoolean() ? stream.ReadString() : null;
        MapGamePawnEmblem? emblem = stream.ReadBoolean() ? MapGamePawnEmblem.Decode(stream) : null;
        int unknownGlobalIdentifier = stream.ReadVariableInt();
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> notifications = CommandDataReferenceVariableIntPairArrayField.Decode(stream).Values;

        return new MapGamePawn(
            avatarIdentifier,
            neighborhoodIdentifier,
            experienceLevel,
            currentNodeIdentifier,
            emptyNodesTravelled,
            sanctuaryAnimalEscapeAllowance,
            sanctuaryAnimalEscapeProgress,
            visitedNodeIdentifiers,
            selectedOptions,
            name,
            emblem,
            unknownGlobalIdentifier,
            notifications
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, AvatarIdentifier);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, NeighborhoodIdentifier);
        stream.WriteVariableInt(ExperienceLevel);
        stream.WriteVariableInt(CurrentNodeIdentifier);
        stream.WriteVariableInt(EmptyNodesTravelled);
        stream.WriteVariableInt(SanctuaryAnimalEscapeAllowance);
        stream.WriteVariableInt(SanctuaryAnimalEscapeProgress);
        new CommandVariableIntArrayField(VisitedNodeIdentifiers).Encode(stream);
        new CommandDataReferenceArrayField(SelectedOptions).Encode(stream);
        stream.WriteBoolean(Name is not null);

        if (Name is not null)
            stream.WriteString(Name);

        stream.WriteBoolean(Emblem is not null);
        Emblem?.Encode(stream);
        stream.WriteVariableInt(UnknownGlobalIdentifier);
        new CommandDataReferenceVariableIntPairArrayField(Notifications).Encode(stream);
    }
}
