namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>
/// <para>Base wire representation shared by Hay Day logic commands.</para>
/// </summary>
public abstract record Command
{
    private CommandData? _debugData0;
    private CommandData? _debugData1;
    private int _executionPhaseCounter = -1;

    /// <summary>Gets the command's contract, its identity across releases; CommandRegistry selects its version-specific wire id.</summary>
    public abstract CommandContract Contract { get; }

    /// <summary>Gets the first diagnostic state native sends outside production.</summary>
    public CommandData? DebugData0
    {
        get => _debugData0;
        init => _debugData0 = value;
    }

    /// <summary>Gets the second diagnostic state native sends outside production.</summary>
    public CommandData? DebugData1
    {
        get => _debugData1;
        init => _debugData1 = value;
    }

    /// <summary>Gets the sub-tick at which the command executes.</summary>
    public int ExecutionPhaseCounter
    {
        get => _executionPhaseCounter;
        init => _executionPhaseCounter = value;
    }

    /// <summary>Sets the sub-tick at which the command executes.</summary>
    public void SetExecutionPhaseCounter(int executionPhaseCounter)
    {
        _executionPhaseCounter = executionPhaseCounter;
    }

    internal void ApplyMetadata(int executionPhaseCounter, CommandData? debugData0, CommandData? debugData1)
    {
        _executionPhaseCounter = executionPhaseCounter;
        _debugData0 = debugData0;
        _debugData1 = debugData1;
    }
}
