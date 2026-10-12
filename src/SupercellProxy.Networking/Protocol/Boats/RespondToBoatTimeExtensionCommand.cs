using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Accepts or declines extra time for an expired boat order.</summary>
public sealed record RespondToBoatTimeExtensionCommand(bool Extend) : Command
{
    /// <summary>Gets the registered command type.</summary>
    public override int Type => CommandRegistry.RespondToBoatTimeExtensionCommandType;

    /// <summary>Reads the native extension decision.</summary>
    public static RespondToBoatTimeExtensionCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new RespondToBoatTimeExtensionCommand(stream.ReadBoolean());
    }

    /// <summary>Writes the native extension decision.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(Extend);
    }
}
