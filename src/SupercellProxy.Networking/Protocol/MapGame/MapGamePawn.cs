using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// Native map-game pawn structure encoded by the shared 1.72.84 helper at 0x10065c78c.
/// Semantic names for the stripped fields are not yet proven.
/// </summary>
public sealed record MapGamePawn : ICodecProvider<MapGamePawn>
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
        int emptyNodesTravelledDeliveringAnimalsImmunity,
        int emptyNodesTravelledDeliveringAnimals,
        ReadOnlyMemory<int> visitedNodeIds,
        ReadOnlyMemory<int> selectedOptions,
        string? name,
        MapGamePawnEmblem? emblem,
        int derbyTaskGlobalId,
        ReadOnlyMemory<CommandDataReferenceVarIntPair> pendingNotifications
    )
    {
        AvatarId = avatarId;
        NeighborhoodId = neighborhoodId;
        ExperienceLevel = experienceLevel;
        CurrentNodeId = currentNodeId;
        EmptyNodesTravelled = emptyNodesTravelled;
        EmptyNodesTravelledDeliveringAnimalsImmunity = emptyNodesTravelledDeliveringAnimalsImmunity;
        EmptyNodesTravelledDeliveringAnimals = emptyNodesTravelledDeliveringAnimals;
        VisitedNodeIds = visitedNodeIds.ToArray();
        SelectedOptions = selectedOptions.ToArray();
        Name = name;
        Emblem = emblem;
        DerbyTaskGlobalId = derbyTaskGlobalId;
        PendingNotifications = pendingNotifications.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGamePawn> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static pawn => pawn.AvatarId,
        Codecs.OptionalLongId,
        static pawn => pawn.NeighborhoodId,
        Codecs.VarInt,
        static pawn => pawn.ExperienceLevel,
        Codecs.VarInt,
        static pawn => pawn.CurrentNodeId,
        Codecs.VarInt,
        static pawn => pawn.EmptyNodesTravelled,
        Codecs.VarInt,
        static pawn => pawn.EmptyNodesTravelledDeliveringAnimalsImmunity,
        Codecs.VarInt,
        static pawn => pawn.EmptyNodesTravelledDeliveringAnimals,
        Codecs.Memory(Codecs.VarInt),
        static pawn => pawn.VisitedNodeIds,
        Codecs.Memory(Codecs.VarInt),
        static pawn => pawn.SelectedOptions,
        Codecs.Optional(Codecs.String),
        static pawn => pawn.Name,
        Codecs.Optional(MapGamePawnEmblem.Codec),
        static pawn => pawn.Emblem,
        Codecs.VarInt,
        static pawn => pawn.DerbyTaskGlobalId,
        Codecs.Memory(CommandDataReferenceVarIntPair.Codec, CommandVarIntPair.ArrayCount),
        static pawn => pawn.PendingNotifications,
        static (
            avatarId,
            neighborhoodId,
            experienceLevel,
            currentNodeId,
            emptyNodesTravelled,
            emptyNodesTravelledDeliveringAnimalsImmunity,
            emptyNodesTravelledDeliveringAnimals,
            visitedNodeIds,
            selectedOptions,
            name,
            emblem,
            derbyTaskGlobalId,
            pendingNotifications
        ) => new MapGamePawn(
            avatarId,
            neighborhoodId,
            experienceLevel,
            currentNodeId,
            emptyNodesTravelled,
            emptyNodesTravelledDeliveringAnimalsImmunity,
            emptyNodesTravelledDeliveringAnimals,
            visitedNodeIds,
            selectedOptions,
            name,
            emblem,
            derbyTaskGlobalId,
            pendingNotifications
        )
    );

    /// <summary>
    /// Gets the <c language="csharp">AvatarId</c> value.
    /// </summary>
    public LongId? AvatarId { get; }

    /// <summary>
    /// Gets the <c language="csharp">CurrentNodeId</c> value.
    /// </summary>
    public int CurrentNodeId { get; }

    /// <summary>
    /// Gets the derby task the pawn works on (native JSON <c>DerbyTaskGlobalId</c>).
    /// </summary>
    public int DerbyTaskGlobalId { get; init; }

    /// <summary>
    /// Gets the participant's emblem.
    /// </summary>
    public MapGamePawnEmblem? Emblem { get; init; }

    /// <summary>
    /// Gets the number of empty nodes travelled.
    /// </summary>
    public int EmptyNodesTravelled { get; }

    /// <summary>
    /// Gets the empty nodes travelled while carrying sanctuary animals (native JSON <c>EmptyNodesTravelledDeliveringAnimals</c>).
    /// </summary>
    public int EmptyNodesTravelledDeliveringAnimals { get; }

    /// <summary>
    /// Gets the remaining empty-node immunity before a carried sanctuary animal can escape (native JSON <c>EmptyNodesTravelledDeliveringAnimalsImmunity</c>).
    /// </summary>
    public int EmptyNodesTravelledDeliveringAnimalsImmunity { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExperienceLevel</c> value.
    /// </summary>
    public int ExperienceLevel { get; init; }

    /// <summary>
    /// Gets the participant's name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the participant's neighborhood id.
    /// </summary>
    public LongId? NeighborhoodId { get; init; }

    /// <summary>
    /// Gets the pending pendingNotifications (native JSON <c>PendingNotifications</c>).
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> PendingNotifications { get; init; }

    /// <summary>
    /// Gets the selected map-game profile options.
    /// </summary>
    public ReadOnlyMemory<int> SelectedOptions { get; init; }

    /// <summary>
    /// Gets the ordered, unique node ids revealed by this pawn's movements.
    /// </summary>
    public ReadOnlyMemory<int> VisitedNodeIds { get; }
}
