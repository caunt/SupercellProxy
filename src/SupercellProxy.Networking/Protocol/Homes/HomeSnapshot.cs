using SupercellProxy.Networking.Protocol.Expansion;
using SupercellProxy.Networking.Protocol.GameObjects;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents decoded <c language="csharp">HomeSnapshot</c> home data.
/// </summary>
public sealed record HomeSnapshot
{
    /// <summary>
    /// Gets or sets every object placed in the area: buildings, fields, trees, animals, decorations and the like.
    /// </summary>
    /// <value>The saved objects in the document's order, each with its own data id, position and state; empty by default.</value>
    public GameObjectSnapshot[] Objects { get; init; } = [];

    /// <summary>
    /// Gets or sets the saved object id lists, one per action kind; their exact role is not identified.
    /// </summary>
    /// <value>One entry per saved <c>Action</c> number (captured farms carry action 5), each with its list of ids; empty by default.</value>
    public GameObjectTimeListSnapshot[] TimeLists { get; init; } = [];

    /// <summary>
    /// Gets or sets the running count of animal products made on the farm, one number per animal product kind.
    /// </summary>
    /// <value>Non-negative totals, one per animal product kind (six entries in captured farms); empty by default.</value>
    public int[] CreatedAnimalProducts { get; init; } = [];
    /// <summary>
    /// Gets or sets the revision of the saved object format (8 in 1.73.81 captures).
    /// </summary>
    /// <value>The format number written by the game that saved the home; older 1.72 captures show 7, and 0 means the key was absent.</value>
    public int ObjectVersion { get; init; }

    /// <summary>
    /// Gets or sets the running count per mine product, one number for each entry of the mine goods table in order.
    /// </summary>
    /// <value>One total per mine goods row, in table order. Empty by default.</value>
    public int[] MineProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the running count per smelter product, one number per product kind.
    /// </summary>
    /// <value>Non-negative totals, one per smelter product kind (five entries, all 0, in captured farms that have not smelted yet); empty by default.</value>
    public int[] SmelterProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the height of the area's tile grid, in tiles.
    /// </summary>
    /// <value>The number of tile rows (86 on captured farms); 0 by default.</value>
    public int TileMapHeight { get; init; }

    /// <summary>
    /// Gets or sets the width of the area's tile grid, in tiles.
    /// </summary>
    /// <value>The number of tile columns (100 on captured farms); 0 by default.</value>
    public int TileMapWidth { get; init; }

    /// <summary>
    /// Gets or sets the land expansions the player has started paying tools into, with the tools already handed in.
    /// </summary>
    /// <value>One entry per partly paid expansion; an expansion missing here has nothing paid. Empty by default.</value>
    public ExpansionReadyDataSnapshot[] ExpansionReadyDatas { get; init; } = [];
}
