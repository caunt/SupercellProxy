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
        int emptyNodesTravelledDeliveringAnimals,
        int emptyNodesTravelledDeliveringAnimalsImmunity,
        ReadOnlyMemory<int> unknownValues,
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
        EmptyNodesTravelledDeliveringAnimals = emptyNodesTravelledDeliveringAnimals;
        EmptyNodesTravelledDeliveringAnimalsImmunity = emptyNodesTravelledDeliveringAnimalsImmunity;
        UnknownValues = unknownValues.ToArray();
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
    /// Gets the number of empty nodes travelled while delivering animals.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown3")]
    public int EmptyNodesTravelledDeliveringAnimals { get; }

    /// <summary>
    /// Gets the animal-delivery empty-node immunity counter.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown4")]
    public int EmptyNodesTravelledDeliveringAnimalsImmunity { get; }

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
    /// Gets the <c language="csharp">UnknownValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int> UnknownValues { get; }

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
        int emptyNodesTravelledDeliveringAnimals = stream.ReadVariableInt();
        int emptyNodesTravelledDeliveringAnimalsImmunity = stream.ReadVariableInt();
        int[] unknownValues = CommandVariableIntArrayField.DecodeValues(stream.ReadVariableInt(), stream);
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
            emptyNodesTravelledDeliveringAnimals,
            emptyNodesTravelledDeliveringAnimalsImmunity,
            unknownValues,
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
        stream.WriteVariableInt(EmptyNodesTravelledDeliveringAnimals);
        stream.WriteVariableInt(EmptyNodesTravelledDeliveringAnimalsImmunity);
        new CommandVariableIntArrayField(UnknownValues).Encode(stream);
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
