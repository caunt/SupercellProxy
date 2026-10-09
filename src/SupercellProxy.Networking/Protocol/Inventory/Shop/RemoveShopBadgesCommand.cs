using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Inventory.Shop;

/// <summary>Removes the supplied data entries from the shop's new-item badge list.</summary>
public sealed record RemoveShopBadgesCommand(int[] DataIds) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RemoveShopBadgesCommandType;

    /// <summary>Decodes the data ids following the command metadata.</summary>
    public static RemoveShopBadgesCommand Decode(MessageStream stream, CommandEnvironment environment)
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
