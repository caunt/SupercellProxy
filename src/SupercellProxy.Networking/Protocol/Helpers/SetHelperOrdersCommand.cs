using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// <summary>Replaces the selected helper's production order quantities.</summary>
public sealed record SetHelperOrdersCommand : CodecCommand<SetHelperOrdersCommand>, ICommandCodecProvider<SetHelperOrdersCommand>
{
    /// <inheritdoc/>
    public SetHelperOrdersCommand(byte helperIndex, ReadOnlyMemory<int> amounts)
    {
        HelperIndex = helperIndex;
        Amounts = amounts.ToArray();
    }

    /// <inheritdoc />
    public static CommandCodec<SetHelperOrdersCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Byte,
        static command => command.HelperIndex,
        Codecs.Memory(Codecs.VarInt, CollectionCount.SignedByte),
        static command => command.Amounts,
        static (helperIndex, amounts) => new SetHelperOrdersCommand(helperIndex, amounts)));

    /// <summary>Gets the new order quantity for each product the helper can make.</summary>
    /// <value>One non-negative count per helper order slot, in slot order, sent with a signed-byte count prefix.</value>
    public ReadOnlyMemory<int> Amounts { get; }

    /// <summary>Gets which helper the orders are for.</summary>
    /// <value>0 for the caretaker or 1 for the miller, written as a single byte before the quantities.</value>
    public byte HelperIndex { get; }
}
