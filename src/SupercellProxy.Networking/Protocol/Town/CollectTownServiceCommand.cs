using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Collects a completed town service and releases its passenger.</summary>
/// <param name="ServiceBuildingGlobalId">Game object id of the town service building whose finished service is collected.</param>
/// <param name="FinishedServiceIndex">Zero-based index into the building's finished-services queue, not its running queue, written as a VarInt after the building id.</param>
public sealed record CollectTownServiceCommand(int ServiceBuildingGlobalId, int FinishedServiceIndex) : CodecCommand<CollectTownServiceCommand>, ICommandCodecProvider<CollectTownServiceCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectTownServiceCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.ServiceBuildingGlobalId,
        Codecs.VarInt,
        static command => command.FinishedServiceIndex,
        static (building, service) => new CollectTownServiceCommand(building, service)));
}
