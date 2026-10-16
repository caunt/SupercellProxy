using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>Updates the state of a shop offer.</summary>
/// <param name="OfferId">Store identifier of the offer whose state changes.</param>
/// <param name="Active">Whether the offer is now on sale.</param>
/// <param name="Timestamp">Unix time stamp the server attaches to the change.</param>
public sealed record ShopOfferStateServerCommand(string? OfferId, bool Active, int Timestamp) : CodecServerCommand<ShopOfferStateServerCommand>, ICommandCodecProvider<ShopOfferStateServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ShopOfferStateServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.OptionalString,
            static command => command.OfferId,
            Codecs.Boolean,
            static command => command.Active,
            Codecs.VarInt,
            static command => command.Timestamp,
            static (offerId, active, timestamp) => new ShopOfferStateServerCommand(offerId, active, timestamp)
        ),
        CommandLayout.MetadataFirst
    );
}
