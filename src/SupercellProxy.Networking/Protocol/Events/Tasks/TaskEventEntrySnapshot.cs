namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Defines the Task Event Entry Snapshot contract.
/// </summary>
public sealed record TaskEventEntrySnapshot
{

    /// <summary>
    /// Gets the Birthday Task State value.
    /// </summary>
    public int? BirthdayTaskState { get; init; }

    /// <summary>
    /// Gets the Effective State value.
    /// </summary>
    public int EffectiveState => BirthdayTaskState ?? TaskState;
    /// <summary>
    /// Gets the Task State value.
    /// </summary>
    public int TaskState { get; init; }

    /// <summary>
    /// Provides the Mark Seen value or operation.
    /// </summary>
    public TaskEventEntrySnapshot MarkSeen()
    {
        return BirthdayTaskState is not null
            ? this with
            {
                BirthdayTaskState = 1,
            }
            : this with
            {
                TaskState = 1,
            };
    }
}
