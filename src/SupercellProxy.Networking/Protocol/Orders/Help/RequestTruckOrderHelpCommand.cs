using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Help;

/// <summary>Asks other players for help with a truck order.</summary>
/// <param name="OrderIndex">The truck order slot index.</param>
/// <param name="HelpKind">The requested help kind; kind 2 requires a neighborhood.</param>
public sealed record RequestTruckOrderHelpCommand(int OrderIndex, int HelpKind) : CodecCommand<RequestTruckOrderHelpCommand>, ICommandCodecProvider<RequestTruckOrderHelpCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestTruckOrderHelpCommand> Codec { get; } = Codecs.Command(
        type: 259,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.OrderIndex,
            Codecs.VarInt,
            static command => command.HelpKind,
            static (orderIndex, helpKind) => new RequestTruckOrderHelpCommand(orderIndex, helpKind)
        )
    );
}
