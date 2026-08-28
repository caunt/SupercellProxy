namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Tracks <c language="csharp">CommandExecutionState</c> during turn simulation.
/// </summary>
internal sealed class CommandExecutionState
{
    private readonly List<Command> _commands = [];
    private int _executedCommandCount;

    /// <summary>
    /// Gets the <c language="csharp">ExecutedCommandCount</c> value.
    /// </summary>
    public int ExecutedCommandCount => _executedCommandCount;

    /// <summary>
    /// Gets the <c language="csharp">PendingCommandCount</c> value.
    /// </summary>
    public int PendingCommandCount => _commands.Count;

    /// <summary>
    /// Executes the <c language="csharp">MarkExecuted</c> operation.
    /// </summary>
    public void MarkExecuted(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        _commands.Add(command);
        _executedCommandCount = unchecked(_executedCommandCount + 1);
    }

    /// <summary>
    /// Gets <c language="csharp">PendingCommands</c>.
    /// </summary>
    public Command[] GetPendingCommands()
    {
        return _commands.ToArray();
    }

    /// <summary>
    /// Executes the <c language="csharp">MarkSent</c> operation.
    /// </summary>
    public void MarkSent(ReadOnlySpan<Command> sentCommands)
    {
        if (sentCommands.Length > _commands.Count)
            throw new InvalidOperationException("A turn contains more commands than are pending.");

        for (var i = 0; i < sentCommands.Length; i++)
        {
            if (!ReferenceEquals(_commands[i], sentCommands[i]))
                throw new InvalidOperationException(
                    "A turn does not contain the pending command prefix."
                );
        }

        _commands.RemoveRange(0, sentCommands.Length);
    }
}
