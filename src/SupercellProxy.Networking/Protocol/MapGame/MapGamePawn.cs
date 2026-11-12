using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// One player's car on the Valley board as the server sends it: where it stands, which nodes it revealed, and how it looks.
/// </summary>
public sealed record MapGamePawn : ICodecProvider<MapGamePawn>
{
    /// <summary>Initializes a new instance of the <see cref="MapGamePawn"/> record. Creates a pawn from its decoded fields.</summary>
    /// <param name="avatarId">Account of the player the car belongs to.</param>
    /// <param name="neighborhoodId">Neighbourhood of that player; <see langword="null"/> when they are in none.</param>
    /// <param name="experienceLevel">Farm level of that player.</param>
    /// <param name="currentNodeId">Board node the car stands on.</param>
    /// <param name="emptyNodesTravelled">Empty nodes the car crossed since its last empty-node task.</param>
    /// <param name="emptyNodesTravelledDeliveringAnimalsImmunity">Empty nodes left before a carried sanctuary animal can escape.</param>
    /// <param name="emptyNodesTravelledDeliveringAnimals">Empty nodes crossed while carrying sanctuary animals.</param>
    /// <param name="visitedNodeIds">Nodes the car revealed, in order and without repeats.</param>
    /// <param name="selectedOptions">Valley profile options the player picked.</param>
    /// <param name="name">Name shown for the player.</param>
    /// <param name="emblem">The player's emblem; <see langword="null"/> when none is set.</param>
    /// <param name="derbyTaskGlobalId">Global id of the derby task the car works on; 0 when none.</param>
    /// <param name="pendingNotifications">Valley notifications waiting to be shown, each a table entry and a value.</param>
    public MapGamePawn(LongId? avatarId,
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
        ReadOnlyMemory<CommandDataReferenceVarIntPair> pendingNotifications)
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
    public static ICodec<MapGamePawn> Codec { get; } = Codecs.Composite(Codecs.OptionalLongId,
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
        static (avatarId,
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
            pendingNotifications) => new MapGamePawn(avatarId,
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
                pendingNotifications));

    /// <summary>Gets the account of the player the car belongs to.</summary>
    /// <value>The player's account id, the first field on the wire, which identifies the car on the board; <see langword="null"/> when the wire marks it absent.</value>
    public LongId? AvatarId { get; }

    /// <summary>Gets the board node the car stands on.</summary>
    /// <value>The id of a node of the season's Valley map.</value>
    public int CurrentNodeId { get; }

    /// <summary>
    /// Gets or sets the derby task the pawn works on (native JSON <c>DerbyTaskGlobalId</c>).
    /// </summary>
    /// <value>The global id of the derby task row the car is assigned to, sent after <see cref="Emblem"/>; 0 when it works on none.</value>
    public int DerbyTaskGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the participant's emblem.
    /// </summary>
    /// <value>The background, pattern and symbol shown on the car; <see langword="null"/> when the wire marks it absent.</value>
    public MapGamePawnEmblem? Emblem { get; init; }

    /// <summary>Gets the empty nodes the car crossed since its last empty-node task.</summary>
    /// <value>A non-negative count.</value>
    public int EmptyNodesTravelled { get; }

    /// <summary>
    /// Gets the empty nodes travelled while carrying sanctuary animals (native JSON <c>EmptyNodesTravelledDeliveringAnimals</c>).
    /// </summary>
    /// <value>Empty nodes crossed with sanctuary animals on board.</value>
    public int EmptyNodesTravelledDeliveringAnimals { get; }

    /// <summary>
    /// Gets the remaining empty-node immunity before a carried sanctuary animal can escape (native JSON <c>EmptyNodesTravelledDeliveringAnimalsImmunity</c>).
    /// </summary>
    /// <value>Empty nodes the car may still cross safely; 0 when the car carries none.</value>
    public int EmptyNodesTravelledDeliveringAnimalsImmunity { get; }

    /// <summary>Gets or sets the farm level of the player the car belongs to.</summary>
    /// <value>The player's experience level, sent right after <see cref="NeighborhoodId"/>.</value>
    public int ExperienceLevel { get; init; }

    /// <summary>
    /// Gets or sets the participant's name.
    /// </summary>
    /// <value>The player's display name; <see langword="null"/> when the wire marks it absent.</value>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the participant's neighborhood id.
    /// </summary>
    /// <value>The id of the player's neighbourhood, sent second; <see langword="null"/> when the player is in none.</value>
    public LongId? NeighborhoodId { get; init; }

    /// <summary>Gets or sets the Valley notifications waiting to be shown, each a table entry and a value (native JSON <c>PendingNotifications</c>).</summary>
    /// <value>Pairs of a notification row's global id and its value, the last field on the wire; empty when nothing is waiting.</value>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> PendingNotifications { get; init; }

    /// <summary>
    /// Gets or sets the selected map-game profile options.
    /// </summary>
    /// <value>The option numbers the player picked, sent after <see cref="VisitedNodeIds"/>.</value>
    public ReadOnlyMemory<int> SelectedOptions { get; init; }

    /// <summary>
    /// Gets the ordered, unique node ids revealed by this pawn's movements.
    /// </summary>
    /// <value>Node ids in the order the car first reached them, each listed once.</value>
    public ReadOnlyMemory<int> VisitedNodeIds { get; }
}
