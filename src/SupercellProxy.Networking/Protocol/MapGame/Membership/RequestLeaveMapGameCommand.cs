using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Membership;

/// <summary>Requests to leave the valley map game.</summary>
public sealed record RequestLeaveMapGameCommand : CodecCommand<RequestLeaveMapGameCommand>, ICommandCodecProvider<RequestLeaveMapGameCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestLeaveMapGameCommand> Codec { get; } = Codecs.Command(type: 529, Codecs.Unit(static () => new RequestLeaveMapGameCommand()));
}
