using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Requests a quantity of one item from the player's neighborhood.</summary>
public sealed record RequestNeighborhoodItemCommand(int ItemGlobalId, int Quantity) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RequestNeighborhoodItemCommandType;

    /// <summary>Decodes the item and quantity.</summary>
    public static RequestNeighborhoodItemCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int item = stream.ReadVarInt();
        int quantity = stream.ReadVarInt();

        return new RequestNeighborhoodItemCommand(item, quantity);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ItemGlobalId);
        stream.WriteVarInt(Quantity);
    }
}
