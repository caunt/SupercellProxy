using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory.Shop;

/// <summary>Adds the supplied data entries to the shop's new-item badge list.</summary>
public sealed record AddShopBadgesCommand(int[] DataIds) : CodecCommand<AddShopBadgesCommand>, ICommandCodecProvider<AddShopBadgesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AddShopBadgesCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
            static command => command.DataIds,
            static (dataIds) => new AddShopBadgesCommand(dataIds)
        ),
        CommandLayout.MetadataFirst
    );
}
