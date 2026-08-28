namespace SupercellProxy.Playground.Home;

internal sealed class AmbientAnimalSpawnerZoneState
{
    public bool SpawnCycleActive { get; set; }
    public int SpawnDelayThreshold { get; set; }
    public int CleanupDelayThreshold { get; set; }
    public int SpawnActivationCounter { get; set; }
    public int SpawnDelayCounter { get; set; }
    public int SpawnAttemptCounter { get; set; }
    public int CleanupDelayCounter { get; set; }
    public int CleanupActivationInterval { get; set; } = 700;
    public int CleanupPassCounter { get; set; }

    public AmbientAnimalSpawnerZoneLifecycle AdvanceLifecycle(
        int[] configuration,
        int ambientAnimalCount
    )
    {
        SpawnDelayCounter = unchecked(SpawnDelayCounter + 1);

        if (SpawnDelayCounter < SpawnDelayThreshold)
            return default;

        var previousSpawnCounter = SpawnAttemptCounter;
        SpawnAttemptCounter = unchecked(SpawnAttemptCounter + 1);
        CleanupDelayCounter = unchecked(CleanupDelayCounter + 1);

        var spawnRequired = false;

        if (
            previousSpawnCounter >= configuration[4]
            && SpawnActivationCounter < configuration[5]
            && ambientAnimalCount < configuration[11]
        )
        {
            SpawnAttemptCounter = 0;
            SpawnActivationCounter = unchecked(SpawnActivationCounter + 1);
            spawnRequired = true;

            if (SpawnActivationCounter == configuration[5])
                SpawnCycleActive = true;
        }

        if (ambientAnimalCount >= configuration[11] && !SpawnCycleActive)
            SpawnCycleActive = true;

        var cleanupRequired =
            CleanupDelayThreshold is not 0
            && CleanupDelayCounter >= CleanupDelayThreshold
            && ambientAnimalCount > 0;

        if (cleanupRequired)
        {
            CleanupPassCounter = unchecked(CleanupPassCounter + 1);
        }

        return new AmbientAnimalSpawnerZoneLifecycle(
            spawnRequired,
            cleanupRequired,
            IsComplete(configuration, ambientAnimalCount)
        );
    }

    private bool IsComplete(int[] configuration, int ambientAnimalCount)
    {
        return SpawnCycleActive
            && ambientAnimalCount <= configuration[10]
            && CleanupDelayCounter >= CleanupDelayThreshold
            && (CleanupDelayThreshold is 0 || CleanupPassCounter >= CleanupActivationInterval);
    }
}
