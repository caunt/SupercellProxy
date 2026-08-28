namespace SupercellProxy.Playground.Home;

internal sealed class DecorationEventTutorialState
{
    private int? _eventConfigurationOwnerId;

    public int LastIntroEventId { get; private set; } = -1;
    public int LastIntroStep { get; private set; } = -1;

    public static DecorationEventTutorialState Create(DecorationEventTutorialSnapshot? snapshot)
    {
        return new DecorationEventTutorialState
        {
            LastIntroEventId = snapshot?.LastIntroEventId ?? -1,
            LastIntroStep = snapshot?.LastIntroStep ?? -1,
        };
    }

    public void BindEventConfiguration(int eventOwnerId)
    {
        _eventConfigurationOwnerId = eventOwnerId;
    }

    public void ApplyIntroStep(int lastIntroStep)
    {
        if (_eventConfigurationOwnerId is not { } eventOwnerId)
            return;

        LastIntroEventId = eventOwnerId;
        LastIntroStep = lastIntroStep;
    }
}
