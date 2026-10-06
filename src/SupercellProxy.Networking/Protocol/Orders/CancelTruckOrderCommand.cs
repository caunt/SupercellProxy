using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Discards one truck order and starts its free replacement countdown.</summary>
public sealed record CancelTruckOrderCommand(int OrderIndex) : Command
{
    /// <inheritdoc/>
    public override int Type => CommandRegistry.CancelTruckOrderCommandType;

    /// <summary>Reads the order slot; command metadata follows the slot byte.</summary>
    public static CancelTruckOrderCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadByte());
    }

    /// <inheritdoc/>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteByte(checked((byte)OrderIndex));
    }
}
