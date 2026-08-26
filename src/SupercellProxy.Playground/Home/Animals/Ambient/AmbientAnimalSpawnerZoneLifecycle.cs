namespace SupercellProxy.Playground.Home;

internal readonly record struct AmbientAnimalSpawnerZoneLifecycle(
    bool SpawnRequired,
    bool CleanupRequired,
    bool Complete
);
