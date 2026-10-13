using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Requests a quantity of one item from the player's neighborhood.</summary>
public sealed record RequestNeighborhoodItemCommand(int ItemGlobalId, int Quantity) : CodecCommand<RequestNeighborhoodItemCommand>, ICommandCodecProvider<RequestNeighborhoodItemCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestNeighborhoodItemCommand> Codec { get; } = Codecs.Command(
        type: 199,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ItemGlobalId,
            Codecs.VarInt,
            static command => command.Quantity,
            static (item, quantity) => new RequestNeighborhoodItemCommand(item, quantity)
        )
    );
}
