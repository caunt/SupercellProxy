using System.Globalization;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed class GameObjectState(
    int globalId,
    GameObjectSnapshot snapshot,
    DataTableReference data,
    int? tileWidth,
    int? tileHeight
)
{
    internal const int TileCenter = 0x100;
    internal const int TileSize = 0x200;

    public int GlobalId { get; } = globalId;
    public GameObjectSnapshot Snapshot { get; } = snapshot;
    public DataTableReference Data { get; } = data;
    public int? TileWidth { get; } = tileWidth;
    public int? TileHeight { get; } = tileHeight;
    public int PositionX { get; private set; } = GetPosition(snapshot.AccurateX, snapshot.X);
    public int PositionY { get; private set; } = GetPosition(snapshot.AccurateY, snapshot.Y);
    public bool Mirrored { get; private set; } = snapshot.Mirrored;
    public GameObjectState? Parent { get; private set; }

    internal void MoveTo(int x, int y)
    {
        PositionX = x;
        PositionY = y;
    }

    internal void MoveBy(int x, int y)
    {
        PositionX = checked(PositionX + x);
        PositionY = checked(PositionY + y);
    }

    internal void SetMirrored(bool mirrored)
    {
        Mirrored = mirrored;
    }

    internal void AttachTo(GameObjectState parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        if (ReferenceEquals(this, parent))
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Game object {GlobalId} cannot be its own parent."
                )
            );

        if (Parent is not null && !ReferenceEquals(Parent, parent))
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Game object {GlobalId} is already attached to {Parent.GlobalId}."
                )
            );

        Parent = parent;
    }

    private static int GetPosition(int? accuratePosition, int? tilePosition)
    {
        return accuratePosition ?? unchecked(tilePosition.GetValueOrDefault() * TileSize);
    }
}
