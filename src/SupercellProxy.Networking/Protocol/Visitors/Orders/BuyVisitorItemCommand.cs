using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors.Orders;

/// <summary>Buys the item a farm visitor offers.</summary>
/// <param name="VisitorObjectGlobalId">Game object id of the visitor standing on the farm.</param>
public sealed record BuyVisitorItemCommand(int VisitorObjectGlobalId) : CodecCommand<BuyVisitorItemCommand>, ICommandCodecProvider<BuyVisitorItemCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyVisitorItemCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.VisitorObjectGlobalId,
            static visitorObjectGlobalId => new BuyVisitorItemCommand(visitorObjectGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
