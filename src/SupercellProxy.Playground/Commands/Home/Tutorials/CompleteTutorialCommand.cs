using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Marks a tutorial data entry complete.</para>
/// </summary>
internal sealed record CompleteTutorialCommand : Command
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 35;

    /// <summary>
    /// Initializes a new <see cref="CompleteTutorialCommand"/> instance.
    /// </summary>
    public CompleteTutorialCommand(
        int tutorialGlobalId,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        TutorialGlobalId = tutorialGlobalId;
    }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c language="csharp">TutorialGlobalId</c> value.
    /// </summary>
    public int TutorialGlobalId { get; }

    internal static CompleteTutorialCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var tutorialGlobalId = stream.ReadVarInt();
        var fields = DecodeCommand(stream, environment);
        return new CompleteTutorialCommand(
            tutorialGlobalId,
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(TutorialGlobalId);
        EncodeCommand(stream, environment);
    }
}
