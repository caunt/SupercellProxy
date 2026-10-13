using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Collects the finished movie ticket reward.</summary>
public sealed record CollectMovieTicketRewardCommand : CodecCommand<CollectMovieTicketRewardCommand>, ICommandCodecProvider<CollectMovieTicketRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectMovieTicketRewardCommand> Codec { get; } = Codecs.Command(type: 510, Codecs.Unit(static () => new CollectMovieTicketRewardCommand()));
}
