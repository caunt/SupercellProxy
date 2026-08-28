namespace SupercellProxy.Playground.Home;

internal sealed record PathState(
    int ChecksumState0,
    int ChecksumState1,
    int ChecksumState2,
    short[] ChecksumValues,
    int ChecksumCapacity,
    int ChecksumState3,
    int ChecksumState4,
    int ChecksumState5,
    int ChecksumState6,
    int ChecksumState7
)
{
    public static PathState CreateIdle(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);

        return new PathState(-1, -1, 0, [], capacity, 0, 0, 0, 0, 0);
    }
}
