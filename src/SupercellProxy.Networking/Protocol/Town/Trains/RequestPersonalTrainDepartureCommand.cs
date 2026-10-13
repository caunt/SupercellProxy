using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Trains;

/// <summary>Requests the departure of the player's personal train while it waits in a visited town.</summary>
public sealed record RequestPersonalTrainDepartureCommand : CodecCommand<RequestPersonalTrainDepartureCommand>, ICommandCodecProvider<RequestPersonalTrainDepartureCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestPersonalTrainDepartureCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new RequestPersonalTrainDepartureCommand()));
}
