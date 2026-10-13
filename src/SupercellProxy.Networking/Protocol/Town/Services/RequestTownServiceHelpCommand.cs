using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>Asks other players for help with a town service.</summary>
/// <param name="ServiceIndex">Gets the service index inside the building.</param>
/// <param name="ServiceBuildingGlobalId">Gets the service building game object id.</param>
/// <param name="HelpKind">Gets the requested help kind; kind 2 requires a neighborhood.</param>
public sealed record RequestTownServiceHelpCommand(int ServiceIndex, int ServiceBuildingGlobalId, int HelpKind) : CodecCommand<RequestTownServiceHelpCommand>, ICommandCodecProvider<RequestTownServiceHelpCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestTownServiceHelpCommand> Codec { get; } = Codecs.Command(
        type: 250,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ServiceIndex,
            Codecs.VarInt,
            static command => command.ServiceBuildingGlobalId,
            Codecs.VarInt,
            static command => command.HelpKind,
            static (serviceIndex, serviceBuildingGlobalId, helpKind) => new RequestTownServiceHelpCommand(serviceIndex, serviceBuildingGlobalId, helpKind)
        )
    );
}
