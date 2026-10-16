using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>
/// Serverbound command claiming a movie ticket after the player watched its video: the ticket's reward is granted (or, for the
/// double-truck ticket, the truck order pays double) and its play limits advance.
/// </summary>
/// <param name="RequestId">Identifier the client gives this ad view.</param>
/// <param name="OrderIndex">Truck order the double-truck ticket applies to; -1 for tickets that are not about an order.</param>
/// <param name="TicketIndex">Which movie ticket placement was watched; 6 is the double-truck ticket.</param>
/// <param name="ObjectGlobalId">Game object the ticket was used on, for placements tied to an object.</param>
/// <param name="DataGlobalId">Game data entry the ticket was used for, for placements tied to one.</param>
/// <param name="SlotIndex">Slot within that object the ticket was used on, for placements tied to one.</param>
public sealed record ActivateMovieTicketCommand(
    [property: JsonPropertyName("RequestId")] string RequestId,
    int OrderIndex,
    int TicketIndex,
    [property: JsonPropertyName("ObjectGlobalId")] int ObjectGlobalId,
    [property: JsonPropertyName("DataGlobalId")] int DataGlobalId,
    int SlotIndex
)
    : CodecCommand<ActivateMovieTicketCommand>, ICommandCodecProvider<ActivateMovieTicketCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ActivateMovieTicketCommand> Codec { get; } = Codecs.Command(
        Codecs.Versioned(
            Release17381.Version,
            Codecs.Composite(
                Codecs.String,
                static command => command.RequestId,
                Codecs.VarInt,
                static command => command.DataGlobalId,
                Codecs.VarInt,
                static command => command.OrderIndex,
                Codecs.VarInt,
                static command => command.SlotIndex,
                Codecs.VarInt,
                static command => command.ObjectGlobalId,
                Codecs.VarInt,
                static command => command.TicketIndex,
                static (requestId, dataGlobalId, orderIndex, slotIndex, objectGlobalId, ticketIndex) => new ActivateMovieTicketCommand(requestId, orderIndex, ticketIndex, objectGlobalId, dataGlobalId, slotIndex)
            ),
            Codecs.Composite(
                Codecs.String,
                static command => command.RequestId,
                Codecs.VarInt,
                static command => command.OrderIndex,
                Codecs.VarInt,
                static command => command.TicketIndex,
                Codecs.VarInt,
                static command => command.ObjectGlobalId,
                Codecs.VarInt,
                static command => command.DataGlobalId,
                Codecs.VarInt,
                static command => command.SlotIndex,
                static (requestId, orderIndex, ticketIndex, objectGlobalId, dataGlobalId, slotIndex) => new ActivateMovieTicketCommand(requestId, orderIndex, ticketIndex, objectGlobalId, dataGlobalId, slotIndex)
            )
        ),
        CommandLayout.MetadataFirst
    );
}
