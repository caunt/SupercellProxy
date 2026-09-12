using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// <para>Starts the selected tutorial.</para>
/// </summary>
public sealed record StartTutorialCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 35;

    /// <summary>
    /// Initializes a new <see cref="StartTutorialCommand"/> instance.
    /// </summary>
    public StartTutorialCommand(int tutorialGlobalIdentifier, int executionPhaseCounter = -1, CommandData? debugData0 = null, CommandData? debugData1 = null)
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        TutorialGlobalIdentifier = tutorialGlobalIdentifier;
    }

    /// <summary>
    /// Gets the <c language="csharp">TutorialGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TutorialGlobalId")]
    public int TutorialGlobalIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static StartTutorialCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int tutorialGlobalIdentifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new StartTutorialCommand(tutorialGlobalIdentifier, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(TutorialGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
