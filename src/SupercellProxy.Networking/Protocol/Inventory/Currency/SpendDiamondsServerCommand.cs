using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Inventory.Currency;

/// <summary>Applies a server-directed diamond debit.</summary>
public sealed record SpendDiamondsServerCommand(int Amount) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SpendDiamondsServerCommandType;

    /// <summary>Decodes the diamond amount.</summary>
    public static SpendDiamondsServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Amount);
    }
}
