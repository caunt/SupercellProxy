using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class AnimalHabitatGrid
{
    private readonly AnimalHabitatGridCell[] _cells;

    private AnimalHabitatGrid(int width, int height)
    {
        Width = width;
        Height = height;
        _cells = new AnimalHabitatGridCell[checked(width * height)];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
                _cells[x + width * y] = new AnimalHabitatGridCell(x, y);
        }
    }

    public int Width { get; }
    public int Height { get; }

    public static AnimalHabitatGrid Create(
        AnimalHabitatState habitat,
        AnimalHabitatPieceState[] pieces,
        DataTableResolver dataTableResolver
    )
    {
        if (
            !dataTableResolver.TryResolveInt(
                habitat.GameObject.Data.GlobalId,
                "TileWidth",
                out var width
            )
            || !dataTableResolver.TryResolveInt(
                habitat.GameObject.Data.GlobalId,
                "TileHeight",
                out var height
            )
            || width <= 0
            || height <= 0
        )
        {
            throw new InvalidDataException(
                $"Animal habitat {habitat.GameObject.Data.Name} has invalid native grid dimensions."
            );
        }

        var grid = new AnimalHabitatGrid(width, height);

        foreach (
            var piece in pieces.Where(piece =>
                piece.AnimalHabitatGlobalId == habitat.GameObject.GlobalId
            )
        )
        {
            var fence =
                dataTableResolver.TryResolveBoolean(
                    piece.GameObject.Data.GlobalId,
                    "Fence",
                    out var configuredFence
                ) && configuredFence;

            if (fence)
                continue;

            var cell = grid.GetCell(
                piece.GameObject.PositionX >> 9,
                piece.GameObject.PositionY >> 9
            );
            cell.Blocked = true;
            cell.Walkable = false;
        }

        return grid;
    }

    public void Occupy(int x, int y, GameObjectState gameObject, bool occupantPassable)
    {
        var cell = GetCell(x, y);
        cell.Occupant = gameObject;
        cell.Walkable = !cell.Blocked && occupantPassable;
    }

    public void Release(GameObjectState gameObject)
    {
        foreach (var cell in _cells)
        {
            if (!ReferenceEquals(cell.Occupant, gameObject))
                continue;

            cell.Occupant = null;
            cell.Walkable = !cell.Blocked;
        }
    }

    public AnimalHabitatGridCell? Select(GameRandom random, bool requireUnoccupied)
    {
        if (_cells.Length is 0)
            return null;

        var start = random.NextInt(_cells.Length);

        for (var offset = 0; offset < _cells.Length; offset++)
        {
            var cell = _cells[(start + offset) % _cells.Length];

            if (cell.Walkable && (!requireUnoccupied || cell.Occupant is null))
                return cell;
        }

        return null;
    }

    public bool HasWalkablePath(int startX, int startY, int targetX, int targetY)
    {
        if (startX > targetX || startX == targetX && startY > targetY)
            return HasWalkablePath(targetX, targetY, startX, startY);

        var deltaX = targetX - startX;
        var deltaY = targetY - startY;
        var absoluteX = Math.Abs(deltaX);
        var absoluteY = Math.Abs(deltaY);

        if (absoluteX < absoluteY)
            return HasVerticalWalkablePath(startX, startY, targetY, deltaX, absoluteY);

        return HasHorizontalWalkablePath(startX, startY, targetX, deltaY, absoluteX);
    }

    private bool HasVerticalWalkablePath(
        int startX,
        int startY,
        int targetY,
        int deltaX,
        int absoluteY
    )
    {
        var fixedX = startX << 14;
        var xStep = absoluteY is 0 ? 0 : deltaX * 0x4000 / absoluteY;
        var yStep = startY < targetY ? 1 : -1;
        var x = startX;
        var y = startY;

        for (var remaining = absoluteY + 1; remaining > 0; remaining--)
        {
            var nextX = fixedX < 0 ? (fixedX + 0x3fff) >> 14 : fixedX >> 14;

            if (
                !IsWalkable(nextX, y)
                || nextX != x && (!IsWalkable(nextX, startY) || !IsWalkable(x, y))
            )
                return false;

            fixedX += xStep;
            x = nextX;
            startY = y;
            y += yStep;
        }

        return true;
    }

    private bool HasHorizontalWalkablePath(
        int startX,
        int startY,
        int targetX,
        int deltaY,
        int absoluteX
    )
    {
        var fixedY = startY << 14;
        var yDelta = absoluteX is 0 ? 0 : deltaY * 0x4000 / absoluteX;
        var xDelta = startX < targetX ? 1 : -1;
        var currentX = startX;
        var currentY = startY;

        for (var remaining = absoluteX + 1; remaining > 0; remaining--)
        {
            var nextY = fixedY < 0 ? (fixedY + 0x3fff) >> 14 : fixedY >> 14;

            if (
                !IsWalkable(currentX, nextY)
                || nextY != currentY
                    && (!IsWalkable(startX, nextY) || !IsWalkable(currentX, currentY))
            )
            {
                return false;
            }

            fixedY += yDelta;
            startX = currentX;
            currentX += xDelta;
            currentY = nextY;
        }

        return true;
    }

    private bool IsWalkable(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height && _cells[x + Width * y].Walkable;
    }

    private AnimalHabitatGridCell GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Animal habitat grid position {x},{y} is outside {Width}x{Height}."
                )
            );

        return _cells[x + Width * y];
    }
}
