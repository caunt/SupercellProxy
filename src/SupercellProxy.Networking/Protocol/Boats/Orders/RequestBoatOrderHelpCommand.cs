using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Orders;

/// <summary>Asks the neighborhood for help with the boat order crates.</summary>
public sealed record RequestBoatOrderHelpCommand : CodecCommand<RequestBoatOrderHelpCommand>, ICommandCodecProvider<RequestBoatOrderHelpCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestBoatOrderHelpCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new RequestBoatOrderHelpCommand()));
}
