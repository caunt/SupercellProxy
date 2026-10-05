using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Starts a selected service at a town service building.</summary>
public sealed record StartTownServiceCommand(int ServiceIndex, int ServiceBuildingGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.StartTownServiceCommandType;

    /// <summary>Decodes the service index and building.</summary>
    public static StartTownServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int index = stream.ReadVarInt();
        int building = stream.ReadVarInt();

        return new StartTownServiceCommand(index, building);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ServiceIndex);
        stream.WriteVarInt(ServiceBuildingGlobalId);
    }
}
