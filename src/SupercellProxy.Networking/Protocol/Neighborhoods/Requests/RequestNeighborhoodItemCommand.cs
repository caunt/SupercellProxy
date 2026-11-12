using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Requests a quantity of one item from the player's neighborhood.</summary>
/// <param name="ItemGlobalId">Global id of the requested crop, product or tool, written as a VarInt.</param>
/// <param name="Quantity">How many to ask for, written as a VarInt.</param>
public sealed record RequestNeighborhoodItemCommand(int ItemGlobalId, int Quantity) : CodecCommand<RequestNeighborhoodItemCommand>, ICommandCodecProvider<RequestNeighborhoodItemCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestNeighborhoodItemCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.ItemGlobalId,
        Codecs.VarInt,
        static command => command.Quantity,
        static (item, quantity) => new RequestNeighborhoodItemCommand(item, quantity)));
}
