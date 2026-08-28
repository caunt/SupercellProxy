using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Updates the retained tutorial step for the active decoration event.</para>
/// </summary>
internal sealed record DecorationEventTutorialCommand : Command
{
    /// <summary>
    /// <para>Initializes a decoration-event tutorial command.</para>
    /// </summary>
    public DecorationEventTutorialCommand(
        int lastIntroStep = 0,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        LastIntroStep = lastIntroStep;
    }

    /// <inheritdoc />
    public override int Type => 654;

    /// <summary>
    /// <para>Gets the tutorial step selected by the client.</para>
    /// </summary>
    public int LastIntroStep { get; }

    internal static DecorationEventTutorialCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var commandFields = DecodeCommand(stream, environment);
        return new DecorationEventTutorialCommand(
            stream.ReadVarInt(),
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVarInt(LastIntroStep);
    }
}
