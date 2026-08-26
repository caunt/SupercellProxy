namespace SupercellProxy.Playground.Home;

internal sealed class AmbientAnimalSpawnerZoneState
{
    public bool ChecksumFlag0 { get; set; }
    public int ChecksumState0 { get; set; }
    public int ChecksumState1 { get; set; }
    public int ChecksumState2 { get; set; }
    public int ChecksumState3 { get; set; }
    public int ChecksumState4 { get; set; }
    public int ChecksumState5 { get; set; }
    public int ChecksumState6 { get; set; } = 700;
    public int ChecksumState7 { get; set; }

    public AmbientAnimalSpawnerZoneLifecycle AdvanceLifecycle(
        int[] configuration,
        int ambientAnimalCount
    )
    {
        ChecksumState3 = unchecked(ChecksumState3 + 1);

        if (ChecksumState3 < ChecksumState0)
            return default;

        var previousSpawnCounter = ChecksumState4;
        ChecksumState4 = unchecked(ChecksumState4 + 1);
        ChecksumState5 = unchecked(ChecksumState5 + 1);

        var spawnRequired = false;

        if (
            previousSpawnCounter >= configuration[4]
            && ChecksumState2 < configuration[5]
            && ambientAnimalCount < configuration[11]
        )
        {
            ChecksumState4 = 0;
            ChecksumState2 = unchecked(ChecksumState2 + 1);
            spawnRequired = true;

            if (ChecksumState2 == configuration[5])
                ChecksumFlag0 = true;
        }

        if (ambientAnimalCount >= configuration[11] && !ChecksumFlag0)
            ChecksumFlag0 = true;

        var cleanupRequired =
            ChecksumState1 is not 0 && ChecksumState5 >= ChecksumState1 && ambientAnimalCount > 0;

        if (cleanupRequired)
        {
            ChecksumState7 = unchecked(ChecksumState7 + 1);
        }

        return new AmbientAnimalSpawnerZoneLifecycle(
            spawnRequired,
            cleanupRequired,
            IsComplete(configuration, ambientAnimalCount)
        );
    }

    private bool IsComplete(int[] configuration, int ambientAnimalCount)
    {
        return ChecksumFlag0
            && ambientAnimalCount <= configuration[10]
            && ChecksumState5 >= ChecksumState1
            && (ChecksumState1 is 0 || ChecksumState7 >= ChecksumState6);
    }
}
