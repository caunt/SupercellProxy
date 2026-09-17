using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// <para>Collects proceeds from one sold roadside listing.</para>
/// </summary>
public sealed record CollectRoadsideSaleProceedsCommand : Command
{
    /// <summary>
    /// <para>Initializes collection of a sold listing.</para>
    /// </summary>
    public CollectRoadsideSaleProceedsCommand(int slotIndex = 0, int executionPhaseCounter = -1, CommandData? debugData0 = null, CommandData? debugData1 = null)
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        SlotIndex = slotIndex;
    }

    /// <summary>
    /// <para>Gets the sold listing slot index.</para>
    /// </summary>
    public int SlotIndex { get; }

    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectRoadsideSaleProceedsCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CollectRoadsideSaleProceedsCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = DecodeCommand(stream, environment);

        return new CollectRoadsideSaleProceedsCommand(stream.ReadVariableInt(), commandFields.ExecutionPhaseCounter, commandFields.DebugData0, commandFields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(SlotIndex);
    }
}
