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
    /// <value>The contract of the concrete record's codec; a record that carries several native commands returns the one matching its state.</value>
    public abstract CommandContract Contract { get; }

    /// <summary>Gets or sets the first diagnostic state native sends outside production.</summary>
    /// <value>The first optional debug block of the command metadata, or <see langword="null"/> when it is absent, which is always the case in the production environment.</value>
    public CommandData? DebugData0
    {
        get => _debugData0;
        init => _debugData0 = value;
    }

    /// <summary>Gets or sets the second diagnostic state native sends outside production.</summary>
    /// <value>The second optional debug block, written after <see cref="DebugData0"/>, or <see langword="null"/> when it is absent, which is always the case in the production environment.</value>
    public CommandData? DebugData1
    {
        get => _debugData1;
        init => _debugData1 = value;
    }

    /// <summary>Gets or sets the sub-tick at which the command executes.</summary>
    /// <value>The execution phase counter written as a VarInt in the command metadata; -1 until it is set by <see cref="Set"/>, an initializer, or decoding.</value>
    public int ExecutionPhaseCounter
    {
        get => _executionPhaseCounter;
        init => _executionPhaseCounter = value;
    }

    /// <summary>Sets the sub-tick at which the command executes.</summary>
    /// <param name="executionPhaseCounter">The sub-tick to schedule the command on, stored as <see cref="ExecutionPhaseCounter"/> and written in its metadata; it replaces any earlier value.</param>
    public void Set(int executionPhaseCounter)
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
