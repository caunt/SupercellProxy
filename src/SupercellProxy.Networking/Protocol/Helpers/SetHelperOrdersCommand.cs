using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// Replaces the selected helper's production order quantities.
public sealed record SetHelperOrdersCommand : Command
{
    /// <inheritdoc/>
    public SetHelperOrdersCommand(byte helperIndex, ReadOnlyMemory<int> amounts)
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
        ReadOnlyMemory<int> amounts = CommandByteCountedVarIntArrayField.Decode(stream).Values;

        return new SetHelperOrdersCommand(helperIndex, amounts);
    }

    /// <inheritdoc/>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteByte(HelperIndex);
        new CommandByteCountedVarIntArrayField(Amounts).Encode(stream);
    }
}
