using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Membership;

/// <summary>Requests to join the valley map game.</summary>
public sealed record RequestJoinMapGameCommand : CodecCommand<RequestJoinMapGameCommand>, ICommandCodecProvider<RequestJoinMapGameCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestJoinMapGameCommand> Codec { get; } = Codecs.Command(type: 671, Codecs.Unit(static () => new RequestJoinMapGameCommand()));
}
