using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// Replaces the selected helper's production order quantities.
public sealed record SetHelperOrdersCommand : Command
{
    /// <inheritdoc/>
    public SetHelperOrdersCommand(
        byte helperIndex,
        ReadOnlyMemory<int> amounts,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    ) : base(executionPhaseCounter, debugData0, debugData1)
    {
        HelperIndex = helperIndex;
        Amounts = amounts.ToArray();
    }

    /// <inheritdoc/>
    public ReadOnlyMemory<int> Amounts { get; }
    /// <inheritdoc/>
    public byte HelperIndex { get; }
    /// <inheritdoc/>
    public override int Type => CommandRegistry.SetHelperOrdersCommandType;

    /// <inheritdoc/>
    public static SetHelperOrdersCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        byte helperIndex = stream.ReadByte();
        ReadOnlyMemory<int> amounts = CommandByteCountedVariableIntArrayField.Decode(stream).Values;
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new SetHelperOrdersCommand(helperIndex, amounts, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc/>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteByte(HelperIndex);
        new CommandByteCountedVariableIntArrayField(Amounts).Encode(stream);
        EncodeCommand(stream, environment);
    }
}
