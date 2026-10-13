using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Help;

/// <summary>Thanks the player who helped a truck order.</summary>
/// <param name="OrderIndex">The truck order slot index.</param>
/// <param name="HelpId">The help request id; native carries it to the server without reading it.</param>
public sealed record ThankTruckOrderHelperCommand(int OrderIndex, int HelpId) : CodecCommand<ThankTruckOrderHelperCommand>, ICommandCodecProvider<ThankTruckOrderHelperCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ThankTruckOrderHelperCommand> Codec { get; } = Codecs.Command(
        type: 264,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.OrderIndex,
            Codecs.VarInt,
            static command => command.HelpId,
            static (orderIndex, helpId) => new ThankTruckOrderHelperCommand(orderIndex, helpId)
        )
    );
}
