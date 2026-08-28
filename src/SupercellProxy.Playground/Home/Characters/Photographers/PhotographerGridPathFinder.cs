namespace SupercellProxy.Playground.Home;

internal sealed class PhotographerGridPathFinder
{
    private const int MaximumCost = 0x7fff;
    private const int OccupiedCost = 0x400;
    private readonly int _height;
    private readonly bool[] _occupied;
    private readonly int _width;

    public PhotographerGridPathFinder(GameObjectState[] gameObjects, int width, int height)
    {
        ArgumentNullException.ThrowIfNull(gameObjects);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        _width = width;
        _height = height;
        _occupied = new bool[checked(width * height)];

        foreach (var gameObject in gameObjects)
            RegisterFootprint(gameObject);
    }

    public ushort[] Find(
        int startTileX,
        int startTileY,
        int targetTileX,
        int targetTileY,
        int pointCapacity
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pointCapacity);
        var startIndex = GetIndex(startTileX, startTileY);
        var targetIndex = GetIndex(targetTileX, targetTileY);
        var predecessors = new int[_occupied.Length];
        Array.Fill(predecessors, -1);
        var statuses = new byte[_occupied.Length];
        var priorities = new int[_occupied.Length];
        var heap = new List<int> { startIndex };
        statuses[startIndex] = 1;

        while (heap.Count > 0)
        {
            var current = PopMinimum(heap, priorities);
            statuses[current] = 2;

            if (current == targetIndex)
                break;

            var currentY = current / _width;
            var currentX = current - currentY * _width;
            Consider(currentX, currentY - 1);
            Consider(currentX, currentY + 1);
            Consider(currentX - 1, currentY);
            Consider(currentX + 1, currentY);

            void Consider(int x, int y)
            {
                if (x < 0 || y < 0 || x >= _width || y >= _height)
                    return;

                var candidate = y * _width + x;
                if (statuses[candidate] is not 0)
                    return;

                var traversalCost = _occupied[candidate] ? OccupiedCost : 0;
                if (traversalCost >= MaximumCost)
                    return;

                var distance = Math.Abs(targetTileX - x) + Math.Abs(targetTileY - y);
                priorities[candidate] = Math.Min(
                    ushort.MaxValue,
                    checked(traversalCost + 5 + 5 * distance)
                );
                predecessors[candidate] = current;
                statuses[candidate] = 1;
                Push(heap, priorities, candidate);
            }
        }

        var reversed = ResolveReversedPath(predecessors, statuses, startIndex, targetIndex);
        if (reversed.Count is 0 || reversed.Count > pointCapacity)
            return [ushort.CreateTruncating(startIndex), ushort.CreateTruncating(targetIndex)];

        reversed.Add(startIndex);
        reversed.Reverse();
        return reversed.Select(ushort.CreateTruncating).ToArray();
    }

    private static List<int> ResolveReversedPath(
        int[] predecessors,
        byte[] statuses,
        int startIndex,
        int targetIndex
    )
    {
        var result = new List<int>();

        if (targetIndex == startIndex || statuses[targetIndex] is 0)
            return result;

        for (var current = targetIndex; current != startIndex; current = predecessors[current])
        {
            if (current < 0)
                return [];

            result.Add(current);
        }

        return result;
    }

    private static void Push(List<int> heap, int[] priorities, int value)
    {
        heap.Add(value);
        var index = heap.Count - 1;

        while (index > 0)
        {
            var parent = (index - 1) / 2;
            if (priorities[heap[index]] >= priorities[heap[parent]])
                return;

            (heap[parent], heap[index]) = (heap[index], heap[parent]);
            index = parent;
        }
    }

    private static int PopMinimum(List<int> heap, int[] priorities)
    {
        var result = heap[0];
        var last = heap[^1];
        heap.RemoveAt(heap.Count - 1);

        if (heap.Count is 0)
            return result;

        heap[0] = last;
        var index = 0;

        while (checked(index * 2 + 1) < heap.Count)
        {
            var left = checked(index * 2 + 1);
            var right = left + 1;
            var child =
                right < heap.Count && priorities[heap[right]] < priorities[heap[left]]
                    ? right
                    : left;

            if (priorities[heap[child]] >= priorities[heap[index]])
                return result;

            (heap[index], heap[child]) = (heap[child], heap[index]);
            index = child;
        }

        return result;
    }

    private void RegisterFootprint(GameObjectState gameObject)
    {
        if (gameObject.TileWidth is not > 0 || gameObject.TileHeight is not > 0)
            return;

        var width = gameObject.Mirrored ? gameObject.TileHeight.Value : gameObject.TileWidth.Value;
        var height = gameObject.Mirrored ? gameObject.TileWidth.Value : gameObject.TileHeight.Value;
        var startX = gameObject.PositionX >> 9;
        var startY = gameObject.PositionY >> 9;

        for (var y = startY; y < checked(startY + height); y++)
        {
            for (var x = startX; x < checked(startX + width); x++)
            {
                if (x >= 0 && y >= 0 && x < _width && y < _height)
                    _occupied[y * _width + x] = true;
            }
        }
    }

    private int GetIndex(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height)
            throw new ArgumentOutOfRangeException(nameof(x), "Path tile is outside the grid.");

        return y * _width + x;
    }
}
