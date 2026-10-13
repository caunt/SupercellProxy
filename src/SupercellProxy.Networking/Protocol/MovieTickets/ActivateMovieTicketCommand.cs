using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Releases;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>
/// Defines the Activate Movie Ticket Command contract.
/// </summary>
/// <summary>
/// Defines the Request Id contract.
/// </summary>
/// <summary>
/// Defines the Order Index contract.
/// </summary>
/// <summary>
/// Defines the Ticket Index contract.
/// </summary>
/// <summary>
/// Defines the Object Global Id contract.
/// </summary>
/// <summary>
/// Defines the Data Global Id contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
public sealed record ActivateMovieTicketCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("RequestId")] string RequestId,
    int OrderIndex,
    int TicketIndex,
    [property: System.Text.Json.Serialization.JsonPropertyName("ObjectGlobalId")] int ObjectGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("DataGlobalId")] int DataGlobalId,
    int SlotIndex
)
    : CodecCommand<ActivateMovieTicketCommand>, ICommandCodecProvider<ActivateMovieTicketCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ActivateMovieTicketCommand> Codec { get; } = Codecs.Command(
        type: 643,
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
