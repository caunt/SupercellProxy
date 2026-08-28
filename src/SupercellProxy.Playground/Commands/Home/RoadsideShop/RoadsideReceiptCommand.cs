using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Processes one retained roadside-shop receipt.</para>
/// </summary>
internal sealed record RoadsideReceiptCommand : Command
{
    /// <summary>
    /// <para>Initializes a roadside-receipt command.</para>
    /// </summary>
    public RoadsideReceiptCommand(
        int receiptIndex = 0,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        ReceiptIndex = receiptIndex;
    }

    /// <inheritdoc />
    public override int Type => 649;

    /// <summary>
    /// <para>Gets the retained receipt index.</para>
    /// </summary>
    public int ReceiptIndex { get; }

    internal static RoadsideReceiptCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var commandFields = DecodeCommand(stream, environment);
        return new RoadsideReceiptCommand(
            stream.ReadVarInt(),
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVarInt(ReceiptIndex);
    }
}
