using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Clears the movie ticket shop notifications.</summary>
public sealed record ClearMovieTicketShopNotificationsCommand : CodecCommand<ClearMovieTicketShopNotificationsCommand>, ICommandCodecProvider<ClearMovieTicketShopNotificationsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClearMovieTicketShopNotificationsCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ClearMovieTicketShopNotificationsCommand()));
}
