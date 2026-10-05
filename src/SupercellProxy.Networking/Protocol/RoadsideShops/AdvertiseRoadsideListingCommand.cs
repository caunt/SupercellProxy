using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Advertises an existing stand using a free opportunity or a purchased advertisement credit.</summary>
public sealed record AdvertiseRoadsideListingCommand(int SlotIndex) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.AdvertiseRoadsideListingCommandType;
    /// <summary>Decodes the phase and stand index.</summary>
    public static AdvertiseRoadsideListingCommand Decode(MessageStream stream, CommandEnvironment environment)
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
