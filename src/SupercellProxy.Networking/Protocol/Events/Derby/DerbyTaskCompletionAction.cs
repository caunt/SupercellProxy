namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Native stages for recording leaderboard credit and submitting a completed derby task.</summary>
public enum DerbyTaskCompletionAction
{
    /// <summary>No stage: the value 0, which neither records leaderboard points nor submits the task.</summary>
    None = 0,

    /// <summary>Records that the task's points have reached the leaderboard.</summary>
    RecordLeaderboardPoints = 1,
    /// <summary>Submits the completed task and clears the active task.</summary>
    Submit = 2,
}
