using SupercellProxy.Networking.Protocol.Expansion;
using SupercellProxy.Networking.Protocol.GameObjects;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents decoded <c language="csharp">HomeSnapshot</c> home data.
/// </summary>
public sealed record HomeSnapshot
{

    /// <summary>
    /// Gets or sets the <c language="csharp">Objects</c> value.
    /// </summary>
    public GameObjectSnapshot[] Objects { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">TimeLists</c> value.
    /// </summary>
    public GameObjectTimeListSnapshot[] TimeLists { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">CreatedAnimalProducts</c> value.
    /// </summary>
    public int[] CreatedAnimalProducts { get; init; } = [];
    /// <summary>
    /// Gets or sets the <c language="csharp">ObjectVersion</c> value.
    /// </summary>
    public int ObjectVersion { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MineProducts</c> value.
    /// </summary>
    public int[] MineProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">SmelterProducts</c> value.
    /// </summary>
    public int[] SmelterProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">TileMapHeight</c> value.
    /// </summary>
    public int TileMapHeight { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TileMapWidth</c> value.
    /// </summary>
    public int TileMapWidth { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ExpansionReadyDatas</c> value.
    /// </summary>
    public ExpansionReadyDataSnapshot[] ExpansionReadyDatas { get; init; } = [];
}
