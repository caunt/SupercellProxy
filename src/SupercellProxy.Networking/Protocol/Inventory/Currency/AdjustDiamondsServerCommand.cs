using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Inventory.Currency;

/// <summary>Applies a server-directed diamond credit or debit.</summary>
public sealed record AdjustDiamondsServerCommand(int Amount, bool Credit) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.AdjustDiamondsServerCommandType;

    /// <summary>Decodes the amount followed by the credit selector.</summary>
    public static AdjustDiamondsServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadBoolean());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Amount);
        stream.WriteBoolean(Credit);
    }
}
