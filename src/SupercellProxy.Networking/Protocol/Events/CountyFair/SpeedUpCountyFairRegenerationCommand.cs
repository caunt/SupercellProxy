using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.CountyFair;

/// <summary>Pays diamonds to finish the county fair regeneration timer.</summary>
/// <param name="EventId">The county fair event id.</param>
/// <param name="Price">The expected diamond price.</param>
public sealed record SpeedUpCountyFairRegenerationCommand(int EventId, int Price) : CodecCommand<SpeedUpCountyFairRegenerationCommand>, ICommandCodecProvider<SpeedUpCountyFairRegenerationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpCountyFairRegenerationCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.Price,
            static (eventId, price) => new SpeedUpCountyFairRegenerationCommand(eventId, price)
        ),
        CommandLayout.MetadataFirst
    );
}
