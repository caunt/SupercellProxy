using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Cancels an unstarted town service and sends its passenger toward the train.</summary>
public sealed record CancelTownServiceCommand(int ServiceIndex, int ServiceBuildingGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CancelTownServiceCommandType;

    /// <summary>Decodes the running service index and building id.</summary>
    public static CancelTownServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
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
