using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Instantly completes a running town service using premium currency.</summary>
public sealed record SpeedUpTownServiceCommand(int ServiceIndex, int ServiceBuildingGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SpeedUpTownServiceCommandType;

    /// <summary>Decodes the service index, building id, and command fields.</summary>
    public static SpeedUpTownServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int service = stream.ReadVarInt();
        int building = stream.ReadVarInt();

        return new(service, building);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ServiceIndex);
        stream.WriteVarInt(ServiceBuildingGlobalId);
    }
}
