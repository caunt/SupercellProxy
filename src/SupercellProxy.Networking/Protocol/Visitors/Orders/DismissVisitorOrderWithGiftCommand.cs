using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors.Orders;

/// <summary>Dismisses a farm visitor whose order carries a gift.</summary>
/// <param name="VisitorObjectGlobalId">Game object id of the visitor sent away.</param>
public sealed record DismissVisitorOrderWithGiftCommand(int VisitorObjectGlobalId) : CodecCommand<DismissVisitorOrderWithGiftCommand>, ICommandCodecProvider<DismissVisitorOrderWithGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DismissVisitorOrderWithGiftCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.VisitorObjectGlobalId,
            static visitorObjectGlobalId => new DismissVisitorOrderWithGiftCommand(visitorObjectGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
