using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Inventory.Shop;

/// <summary>Adds the supplied data entries to the shop's new-item badge list.</summary>
public sealed record AddShopBadgesCommand(int[] DataIds) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.AddShopBadgesCommandType;

    /// <summary>Decodes the data ids following the command metadata.</summary>
    public static AddShopBadgesCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadArray(static input => input.ReadVarInt()));
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteArray<int>(DataIds, static (writer, id) => writer.WriteVarInt(id));
    }
}
