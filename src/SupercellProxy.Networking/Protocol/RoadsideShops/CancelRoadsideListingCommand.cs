using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Cancels an unsold listing; native configuration determines item return and diamond cost.</summary>
public sealed record CancelRoadsideListingCommand(int SlotIndex) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CancelRoadsideListingCommandType;
    /// <summary>Decodes the phase and stand index.</summary>
    public static CancelRoadsideListingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(SlotIndex);
    }
}
