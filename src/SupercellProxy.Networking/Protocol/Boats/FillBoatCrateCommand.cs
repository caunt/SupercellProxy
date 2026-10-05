using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Fills an own-farm boat crate using goods in the player's inventories.</summary>
/// <param name="UseDirectRewards">Selects direct inventory grants instead of per-resource presentation callbacks. Both paths update state synchronously.</param>
/// <param name="CrateIndex">The zero-based crate within the selected order.</param>
/// <param name="AllowCompletedOrder">Allows filling an unpaid crate after helpers completed the order.</param>
public sealed record FillBoatCrateCommand(bool UseDirectRewards, int CrateIndex, bool AllowCompletedOrder) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.FillBoatCrateCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static FillBoatCrateCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new FillBoatCrateCommand(stream.ReadBoolean(), stream.ReadVarInt(), stream.ReadBoolean());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(UseDirectRewards);
        stream.WriteVarInt(CrateIndex);
        stream.WriteBoolean(AllowCompletedOrder);
    }
}
