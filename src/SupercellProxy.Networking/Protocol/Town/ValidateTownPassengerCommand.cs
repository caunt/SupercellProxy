using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Validates that a passenger is present in the player's town without changing its state.</summary>
public sealed record ValidateTownPassengerCommand(int PassengerGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ValidateTownPassengerCommandType;

    /// <summary>Decodes the passenger id and command fields.</summary>
    public static ValidateTownPassengerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int passenger = stream.ReadVarInt();

        return new(passenger);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(PassengerGlobalId);
    }
}
