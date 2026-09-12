using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// <para>Processes one retained roadside-shop receipt.</para>
/// </summary>
public sealed record RoadsideReceiptCommand : Command
{
    /// <summary>
    /// <para>Initializes a roadside-receipt command.</para>
    /// </summary>
    public RoadsideReceiptCommand(int receiptIndex = 0, int executionPhaseCounter = -1, CommandData? debugData0 = null, CommandData? debugData1 = null)
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        ReceiptIndex = receiptIndex;
    }

    /// <summary>
    /// <para>Gets the retained receipt index.</para>
    /// </summary>
    public int ReceiptIndex { get; }

    /// <inheritdoc />
    public override int Type => 649;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideReceiptCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = DecodeCommand(stream, environment);

        return new RoadsideReceiptCommand(stream.ReadVariableInt(), commandFields.ExecutionPhaseCounter, commandFields.DebugData0, commandFields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(ReceiptIndex);
    }
}
