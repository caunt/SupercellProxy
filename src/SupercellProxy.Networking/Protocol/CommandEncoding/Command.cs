using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// <para>Base wire representation shared by Hay Day logic commands.</para>
/// </summary>
public abstract record Command
{
    private int _executionPhaseCounter = -1;

    /// <summary>
    /// Gets the <c language="csharp">DebugData0</c> value.
    /// </summary>
    public CommandData? DebugData0 { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">DebugData1</c> value.
    /// </summary>
    public CommandData? DebugData1 { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">ExecutionPhaseCounter</c> value.
    /// </summary>
    public int ExecutionPhaseCounter
    {
        get => _executionPhaseCounter;
        init => _executionPhaseCounter = value;
    }

    /// <summary>
    /// Identifies the command contract. CommandRegistry selects its version-specific wire id.
    /// </summary>
    public abstract int Type { get; }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public abstract void Encode(MessageStream stream, CommandEnvironment environment);

    /// <summary>
    /// Provides the Set Execution Phase Counter value or operation.
    /// </summary>
    public void SetExecutionPhaseCounter(int executionPhaseCounter)
    {
        _executionPhaseCounter = executionPhaseCounter;
    }
}
