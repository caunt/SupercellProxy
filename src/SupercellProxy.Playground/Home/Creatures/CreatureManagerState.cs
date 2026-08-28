using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class CreatureManagerState
{
    private const int InitialSelectionCount = 4;
    private bool _dailyResetPending;
    private bool _initialized;

    private CreatureManagerState(bool dailyResetPending)
    {
        this._dailyResetPending = dailyResetPending;
    }

    public static CreatureManagerState Create(
        CreatureManagerSnapshot? snapshot,
        int serverTimestamp
    )
    {
        if (snapshot is null)
            throw new InvalidDataException("The saved state has no creature manager.");

        if (snapshot.LastKnownEventId > 0)
            throw new NotSupportedException("Active creature events are not implemented.");

        if (snapshot.FarmVisitingCatchList.Length is not 0)
            throw new NotSupportedException("Saved visiting-creature catches are not implemented.");

        return new CreatureManagerState(snapshot.DailyMaxSpawnResetTime <= serverTimestamp);
    }

    public void Update(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (_dailyResetPending)
        {
            CompleteDailyReset();
            return;
        }

        if (_initialized)
            return;

        for (var selection = 0; selection < InitialSelectionCount; selection++)
            _ = random.NextInt(int.MaxValue);

        _initialized = true;
    }

    internal void CompleteInitialSimulation()
    {
        if (_dailyResetPending)
            CompleteDailyReset();

        _initialized = true;
    }

    private void CompleteDailyReset()
    {
        // With no active creature event, native own-home processing clears the
        // deferred daily-spawn flag without selecting a definition or coordinates.
        _dailyResetPending = false;
        _initialized = true;
    }
}
