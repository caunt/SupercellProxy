using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Help;

/// <summary>Helps another player's truck order while visiting their farm.</summary>
/// <param name="OrderIndex">The truck order slot index.</param>
/// <param name="HelpId">The help request id; native carries it to the server without reading it.</param>
public sealed record HelpTruckOrderCommand(int OrderIndex, int HelpId) : CodecCommand<HelpTruckOrderCommand>, ICommandCodecProvider<HelpTruckOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<HelpTruckOrderCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.OrderIndex,
            Codecs.VarInt,
            static command => command.HelpId,
            static (orderIndex, helpId) => new HelpTruckOrderCommand(orderIndex, helpId)
        )
    );
}
