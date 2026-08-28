using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Marks an event-board event as seen.</para>
/// </summary>
internal sealed record NewEventBoardEventSeenCommand : Command
{
    /// <summary>
    /// <para>Initializes an event-board seen command.</para>
    /// </summary>
    public NewEventBoardEventSeenCommand(
        int eventId = 0,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        EventId = eventId;
    }

    /// <inheritdoc />
    public override int Type => 34;

    /// <summary>
    /// <para>Gets the event identifier selected by the client.</para>
    /// </summary>
    public int EventId { get; }

    internal static NewEventBoardEventSeenCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var commandFields = DecodeCommand(stream, environment);
        return new NewEventBoardEventSeenCommand(
            stream.ReadVarInt(),
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVarInt(EventId);
    }
}
