namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Tracks <c>CommandExecutionState</c> during turn simulation.
/// </summary>
public sealed class CommandExecutionState
{
    private readonly List<Command> commands = [];
    private int executedCommandCount;

    /// <summary>
    /// Gets the <c>ExecutedCommandCount</c> value.
    /// </summary>
    public int ExecutedCommandCount => executedCommandCount;

    /// <summary>
    /// Gets the <c>PendingCommandCount</c> value.
    /// </summary>
    public int PendingCommandCount => commands.Count;

    /// <summary>
    /// Executes the <c>MarkExecuted</c> operation.
    /// </summary>
    public void MarkExecuted(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        commands.Add(command);
        executedCommandCount = unchecked(executedCommandCount + 1);
    }

    /// <summary>
    /// Gets <c>PendingCommands</c>.
    /// </summary>
    public Command[] GetPendingCommands()
    {
        return commands.ToArray();
    }

    /// <summary>
    /// Executes the <c>MarkSent</c> operation.
    /// </summary>
    public void MarkSent(ReadOnlySpan<Command> sentCommands)
    {
        if (sentCommands.Length > commands.Count)
            throw new InvalidOperationException("A turn contains more commands than are pending.");

        for (var i = 0; i < sentCommands.Length; i++)
        {
            if (!ReferenceEquals(commands[i], sentCommands[i]))
                throw new InvalidOperationException(
                    "A turn does not contain the pending command prefix."
                );
        }

        commands.RemoveRange(0, sentCommands.Length);
    }
}
