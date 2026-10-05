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
        LongId? avatarId,
        LongId? neighborhoodId,
        int experienceLevel,
        int currentNodeId,
        int emptyNodesTravelled,
        int sanctuaryAnimalEscapeAllowance,
        int sanctuaryAnimalEscapeProgress,
        ReadOnlyMemory<int> visitedNodeIds,
        ReadOnlyMemory<int> selectedOptions,
        string? name,
        MapGamePawnEmblem? emblem,
        int unknownGlobalId,
        ReadOnlyMemory<CommandDataReferenceVarIntPair> notifications
    )
    {
        AvatarId = avatarId;
        NeighborhoodId = neighborhoodId;
        ExperienceLevel = experienceLevel;
        CurrentNodeId = currentNodeId;
        EmptyNodesTravelled = emptyNodesTravelled;
        SanctuaryAnimalEscapeAllowance = sanctuaryAnimalEscapeAllowance;
        SanctuaryAnimalEscapeProgress = sanctuaryAnimalEscapeProgress;
        VisitedNodeIds = visitedNodeIds.ToArray();
        SelectedOptions = selectedOptions.ToArray();
        Name = name;
        Emblem = emblem;
        UnknownGlobalId = unknownGlobalId;
        Notifications = notifications.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId0</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId0")]
    public LongId? AvatarId { get; }

    /// <summary>
    /// Gets the <c language="csharp">CurrentNodeId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown1")]
    public int CurrentNodeId { get; }

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
    /// Gets the participant's neighborhood id.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId1")]
    public LongId? NeighborhoodId { get; }

    /// <summary>
    /// Gets the <c language="csharp">Notifications</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownPairs")]
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> Notifications { get; init; }

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
    public int UnknownGlobalId { get; }

    /// <summary>
    /// Gets the ordered, unique node ids revealed by this pawn's movements.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownValues")]
    public ReadOnlyMemory<int> VisitedNodeIds { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGamePawn Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId? avatarId = MapGameFieldCodec.ReadOptionalLongId(stream);
        LongId? neighborhoodId = MapGameFieldCodec.ReadOptionalLongId(stream);
        int experienceLevel = stream.ReadVarInt();
        int currentNodeId = stream.ReadVarInt();
        int emptyNodesTravelled = stream.ReadVarInt();
        int sanctuaryAnimalEscapeAllowance = stream.ReadVarInt();
        int sanctuaryAnimalEscapeProgress = stream.ReadVarInt();
        int[] visitedNodeIds = CommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream);
        ReadOnlyMemory<int> selectedOptions = CommandDataReferenceArrayField.Decode(stream).GlobalIds;
        string? name = stream.ReadBoolean() ? stream.ReadString() : null;
        MapGamePawnEmblem? emblem = stream.ReadBoolean() ? MapGamePawnEmblem.Decode(stream) : null;
        int unknownGlobalId = stream.ReadVarInt();
        ReadOnlyMemory<CommandDataReferenceVarIntPair> notifications = CommandDataReferenceVarIntPairArrayField.Decode(stream).Values;

        return new MapGamePawn(
            avatarId,
            neighborhoodId,
            experienceLevel,
            currentNodeId,
            emptyNodesTravelled,
            sanctuaryAnimalEscapeAllowance,
            sanctuaryAnimalEscapeProgress,
            visitedNodeIds,
            selectedOptions,
            name,
            emblem,
            unknownGlobalId,
            notifications
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        MapGameFieldCodec.WriteOptionalLongId(stream, AvatarId);
        MapGameFieldCodec.WriteOptionalLongId(stream, NeighborhoodId);
        stream.WriteVarInt(ExperienceLevel);
        stream.WriteVarInt(CurrentNodeId);
        stream.WriteVarInt(EmptyNodesTravelled);
        stream.WriteVarInt(SanctuaryAnimalEscapeAllowance);
        stream.WriteVarInt(SanctuaryAnimalEscapeProgress);
        new CommandVarIntArrayField(VisitedNodeIds).Encode(stream);
        new CommandDataReferenceArrayField(SelectedOptions).Encode(stream);
        stream.WriteBoolean(Name is not null);

        if (Name is not null)
            stream.WriteString(Name);

        stream.WriteBoolean(Emblem is not null);
        Emblem?.Encode(stream);
        stream.WriteVarInt(UnknownGlobalId);
        new CommandDataReferenceVarIntPairArrayField(Notifications).Encode(stream);
    }
}
