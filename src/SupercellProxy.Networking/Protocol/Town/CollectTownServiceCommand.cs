using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Collects a completed town service and releases its passenger.</summary>
public sealed record CollectTownServiceCommand(int ServiceBuildingGlobalId, int FinishedServiceIndex) : CodecCommand<CollectTownServiceCommand>, ICommandCodecProvider<CollectTownServiceCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectTownServiceCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ServiceBuildingGlobalId,
            Codecs.VarInt,
            static command => command.FinishedServiceIndex,
            static (building, service) => new CollectTownServiceCommand(building, service)
        )
    );
}
