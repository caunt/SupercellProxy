using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// Replaces the selected helper's production order quantities.
public sealed record SetHelperOrdersCommand : CodecCommand<SetHelperOrdersCommand>, ICommandCodecProvider<SetHelperOrdersCommand>
{
    /// <inheritdoc/>
    public SetHelperOrdersCommand(byte helperIndex, ReadOnlyMemory<int> amounts)
    {
        HelperIndex = helperIndex;
        Amounts = amounts.ToArray();
    }

    /// <inheritdoc />
    public static CommandCodec<SetHelperOrdersCommand> Codec { get; } = Codecs.Command(
        type: 205,
        Codecs.Composite(
            Codecs.Byte,
            static command => command.HelperIndex,
            Codecs.Memory(Codecs.VarInt, CollectionCount.SignedByte),
            static command => command.Amounts,
            static (helperIndex, amounts) => new SetHelperOrdersCommand(helperIndex, amounts)
        )
    );

    /// <inheritdoc/>
    public ReadOnlyMemory<int> Amounts { get; }

    /// <inheritdoc/>
    public byte HelperIndex { get; }
}
