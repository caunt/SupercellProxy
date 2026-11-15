using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory.Shop;

/// <summary>Removes the supplied data entries from the shop's new-item badge list.</summary>
/// <param name="DataIds">Global ids of the table rows whose badges to clear, written as a counted list of VarInts.</param>
public sealed record RemoveShopBadgesCommand(ImmutableArray<int> DataIds) : CodecCommand<RemoveShopBadgesCommand>, ICommandCodecProvider<RemoveShopBadgesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RemoveShopBadgesCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
            static command => command.DataIds,
            static dataIds => new RemoveShopBadgesCommand(dataIds)),
        CommandLayout.MetadataFirst);
}
