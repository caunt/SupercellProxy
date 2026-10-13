using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.CountyFair;

/// <summary>Uses a county fair lucky bonus ticket.</summary>
/// <param name="EventId">The event id.</param>
public sealed record UseCountyFairLuckyTicketCommand(int EventId) : CodecCommand<UseCountyFairLuckyTicketCommand>, ICommandCodecProvider<UseCountyFairLuckyTicketCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UseCountyFairLuckyTicketCommand> Codec { get; } = Codecs.Command(
        type: 656,
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static (eventId) => new UseCountyFairLuckyTicketCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
