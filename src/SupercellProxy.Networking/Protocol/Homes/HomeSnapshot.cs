using SupercellProxy.Networking.Protocol.Expansion;
using SupercellProxy.Networking.Protocol.GameObjects;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents decoded <c language="csharp">HomeSnapshot</c> home data.
/// </summary>
public sealed record HomeSnapshot
{
    /// <summary>
    /// Every object placed in the area: buildings, fields, trees, animals, decorations and the like.
    /// </summary>
    public GameObjectSnapshot[] Objects { get; init; } = [];

    /// <summary>
    /// Saved object id lists, one per action kind; their exact role is not identified.
    /// </summary>
    public GameObjectTimeListSnapshot[] TimeLists { get; init; } = [];

    /// <summary>
    /// Running count of animal products made on the farm, one number per animal product kind.
    /// </summary>
    public int[] CreatedAnimalProducts { get; init; } = [];
    /// <summary>
    /// Revision of the saved object format (8 in 1.73.81 captures).
    /// </summary>
    public int ObjectVersion { get; init; }

    /// <summary>
    /// Running count per mine product, one number for each entry of the mine goods table in order; the mine's draws depend on them.
    /// </summary>
    public int[] MineProducts { get; init; } = [];

    /// <summary>
    /// Running count per smelter product, one number per product kind.
    /// </summary>
    public int[] SmelterProducts { get; init; } = [];

    /// <summary>
    /// Height of the area's tile grid, in tiles.
    /// </summary>
    public int TileMapHeight { get; init; }

    /// <summary>
    /// Width of the area's tile grid, in tiles.
    /// </summary>
    public int TileMapWidth { get; init; }

    /// <summary>
    /// Land expansions the player has started paying tools into, with the tools already handed in.
    /// </summary>
    public ExpansionReadyDataSnapshot[] ExpansionReadyDatas { get; init; } = [];
}
