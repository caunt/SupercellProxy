namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Saved display state of one task in a task event's list. Birthday events keep the state in their own field.
/// </summary>
public sealed record TaskEventEntrySnapshot
{
    /// <summary>
    /// The task's state when it belongs to a birthday event; null for other events, which use <see cref="TaskState"/>.
    /// </summary>
    public int? BirthdayTaskState { get; init; }

    /// <summary>
    /// The state that applies to this task: the birthday state when present, otherwise <see cref="TaskState"/>.
    /// </summary>
    public int EffectiveState => BirthdayTaskState ?? TaskState;
    /// <summary>
    /// The task's state for non-birthday events; 1 once the player has seen it.
    /// </summary>
    public int TaskState { get; init; }

    /// <summary>
    /// Returns a copy marked as seen (state 1), in whichever field this entry uses.
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
