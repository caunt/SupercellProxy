using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Collects a completed town service and releases its passenger.</summary>
public sealed record CollectTownServiceCommand(int ServiceBuildingGlobalId, int FinishedServiceIndex) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectTownServiceCommandType;

    /// <summary>Decodes the building id, finished-service index, and command fields.</summary>
    public static CollectTownServiceCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int building = stream.ReadVarInt();
        int service = stream.ReadVarInt();

        return new(building, service);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ServiceBuildingGlobalId);
        stream.WriteVarInt(FinishedServiceIndex);
    }
}
