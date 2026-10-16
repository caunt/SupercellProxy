using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// One player's car on the Valley board as the server sends it: where it stands, which nodes it revealed, and how it looks.
/// </summary>
public sealed record MapGamePawn : ICodecProvider<MapGamePawn>
{
    /// <summary>Creates a pawn from its decoded fields.</summary>
    /// <param name="avatarId">Account of the player the car belongs to.</param>
    /// <param name="neighborhoodId">Neighbourhood of that player; null when they are in none.</param>
    /// <param name="experienceLevel">Farm level of that player.</param>
    /// <param name="currentNodeId">Board node the car stands on.</param>
    /// <param name="emptyNodesTravelled">Empty nodes the car crossed since its last empty-node task.</param>
    /// <param name="emptyNodesTravelledDeliveringAnimalsImmunity">Empty nodes left before a carried sanctuary animal can escape.</param>
    /// <param name="emptyNodesTravelledDeliveringAnimals">Empty nodes crossed while carrying sanctuary animals.</param>
    /// <param name="visitedNodeIds">Nodes the car revealed, in order and without repeats.</param>
    /// <param name="selectedOptions">Valley profile options the player picked.</param>
    /// <param name="name">Name shown for the player.</param>
    /// <param name="emblem">The player's emblem; null when none is set.</param>
    /// <param name="derbyTaskGlobalId">Global id of the derby task the car works on; 0 when none.</param>
    /// <param name="pendingNotifications">Valley notifications waiting to be shown, each a table entry and a value.</param>
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

    /// <summary>Account of the player the car belongs to.</summary>
    public LongId? AvatarId { get; }

    /// <summary>Board node the car stands on.</summary>
    public int CurrentNodeId { get; }

    /// <summary>
    /// Gets the derby task the pawn works on (native JSON <c>DerbyTaskGlobalId</c>).
    /// </summary>
    public int DerbyTaskGlobalId { get; init; }

    /// <summary>
    /// Gets the participant's emblem.
    /// </summary>
    public MapGamePawnEmblem? Emblem { get; init; }

    /// <summary>Empty nodes the car crossed since its last empty-node task; it starts over when such a task appears.</summary>
    public int EmptyNodesTravelled { get; }

    /// <summary>
    /// Gets the empty nodes travelled while carrying sanctuary animals (native JSON <c>EmptyNodesTravelledDeliveringAnimals</c>).
    /// </summary>
    public int EmptyNodesTravelledDeliveringAnimals { get; }

    /// <summary>
    /// Gets the remaining empty-node immunity before a carried sanctuary animal can escape (native JSON <c>EmptyNodesTravelledDeliveringAnimalsImmunity</c>).
    /// </summary>
    public int EmptyNodesTravelledDeliveringAnimalsImmunity { get; }

    /// <summary>Farm level of the player the car belongs to.</summary>
    public int ExperienceLevel { get; init; }

    /// <summary>
    /// Gets the participant's name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the participant's neighborhood id.
    /// </summary>
    public LongId? NeighborhoodId { get; init; }

    /// <summary>Valley notifications waiting to be shown, each a table entry and a value (native JSON <c>PendingNotifications</c>).</summary>
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
