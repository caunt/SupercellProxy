namespace SupercellProxy.Playground.Home;

internal sealed record CarPathState(int X, int Y, int PointIndex, int ChecksumState0, int[] Points)
{
    public static CarPathState Create(params int[] points) => new(0, 0, 0, 0, points);
}
