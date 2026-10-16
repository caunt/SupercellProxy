using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>Collects the help given to a waiting town service, which starts the service without spending the owner's goods.</summary>
/// <param name="ServiceIndex">The service slot in the building.</param>
/// <param name="ServiceBuildingGlobalId">Global id of the placed building's game object in the own town.</param>
public sealed record CollectTownServiceHelpCommand(int ServiceIndex, int ServiceBuildingGlobalId) : CodecCommand<CollectTownServiceHelpCommand>, ICommandCodecProvider<CollectTownServiceHelpCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectTownServiceHelpCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ServiceIndex,
            Codecs.VarInt,
            static command => command.ServiceBuildingGlobalId,
            static (serviceIndex, serviceBuildingGlobalId) => new CollectTownServiceHelpCommand(serviceIndex, serviceBuildingGlobalId)
        )
    );
}
