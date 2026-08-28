using SupercellProxy.Playground.Commands;

namespace SupercellProxy.Playground.Home;

internal sealed class DecorationEventManagerState
{
    public DecorationEventTutorialState Tutorial { get; private init; } =
        DecorationEventTutorialState.Create(snapshot: null);
    public int ChallengesOnSubmission { get; private init; }
    public int SubmissionTime { get; private init; }
    public int LastEventId { get; private init; }
    public int EventOwnerId { get; private init; }
    public int EventIdentifier { get; private init; }
    public int Likes { get; private init; }
    public int FeaturingGroup { get; private init; }
    public int PendingReclaimEventId { get; private init; }
    public int LastEventState { get; private init; }

    public static DecorationEventManagerState Create(DecorationEventManagerSnapshot? snapshot)
    {
        var state = new DecorationEventManagerState
        {
            Tutorial = DecorationEventTutorialState.Create(snapshot?.Tutorial),
            ChallengesOnSubmission = snapshot?.ChallengesOnSubmission ?? 0,
            SubmissionTime = snapshot?.SubmissionTime ?? 0,
            LastEventId = snapshot?.LastEventId ?? 0,
            EventOwnerId = snapshot?.EventOwnerId ?? 0,
            EventIdentifier = snapshot?.EventIdentifier ?? 0,
            Likes = snapshot?.Likes ?? 0,
            FeaturingGroup = snapshot?.FeaturingGroup ?? 0,
            PendingReclaimEventId = snapshot?.PendingReclaimEventId ?? 0,
            LastEventState = snapshot?.LastEventState ?? 0,
        };
        if (state.EventOwnerId > 0)
            state.Tutorial.BindEventConfiguration(state.EventOwnerId);

        return state;
    }

    public void ApplyTutorialCommand(DecorationEventTutorialCommand command)
    {
        Tutorial.ApplyIntroStep(command.LastIntroStep);
    }
}
