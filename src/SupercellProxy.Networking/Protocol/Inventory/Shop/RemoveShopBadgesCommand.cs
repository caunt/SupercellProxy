using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory.Shop;

/// <summary>Removes the supplied data entries from the shop's new-item badge list.</summary>
public sealed record RemoveShopBadgesCommand(int[] DataIds) : CodecCommand<RemoveShopBadgesCommand>, ICommandCodecProvider<RemoveShopBadgesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RemoveShopBadgesCommand> Codec { get; } = Codecs.Command(
        type: 711,
        Codecs.Composite(
            Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
            static command => command.DataIds,
            static (dataIds) => new RemoveShopBadgesCommand(dataIds)
        ),
        CommandLayout.MetadataFirst
    );
}
