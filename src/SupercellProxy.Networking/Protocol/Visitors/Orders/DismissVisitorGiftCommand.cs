using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors.Orders;

/// <summary>Dismisses the gift of a farm visitor order.</summary>
/// <param name="VisitorObjectGlobalId">Game object id of the visitor whose gift is declined.</param>
public sealed record DismissVisitorGiftCommand(int VisitorObjectGlobalId) : CodecCommand<DismissVisitorGiftCommand>, ICommandCodecProvider<DismissVisitorGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DismissVisitorGiftCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.VisitorObjectGlobalId,
            static visitorObjectGlobalId => new DismissVisitorGiftCommand(visitorObjectGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
