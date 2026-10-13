using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>Buys a catalogue gift with diamonds on the own farm.</summary>
/// <param name="GiftGlobalId">The catalogue gift data id.</param>
public sealed record BuyCatalogueGiftWithDiamondsCommand(int GiftGlobalId) : CodecCommand<BuyCatalogueGiftWithDiamondsCommand>, ICommandCodecProvider<BuyCatalogueGiftWithDiamondsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyCatalogueGiftWithDiamondsCommand> Codec { get; } = Codecs.Command(
        type: 107,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GiftGlobalId,
            static (giftGlobalId) => new BuyCatalogueGiftWithDiamondsCommand(giftGlobalId)
        )
    );
}
