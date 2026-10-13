using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>Helps another player's town service while visiting their town.</summary>
/// <param name="HelpId">Gets the help request id carried to the server.</param>
/// <param name="ServiceIndex">Gets the service index inside the building.</param>
/// <param name="ServiceBuildingGlobalId">Gets the service building game object id.</param>
public sealed record HelpTownServiceCommand(int HelpId, int ServiceIndex, int ServiceBuildingGlobalId) : CodecCommand<HelpTownServiceCommand>, ICommandCodecProvider<HelpTownServiceCommand>
{
    /// <inheritdoc />
    public static CommandCodec<HelpTownServiceCommand> Codec { get; } = Codecs.Command(
        type: 251,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HelpId,
            Codecs.VarInt,
            static command => command.ServiceIndex,
            Codecs.VarInt,
            static command => command.ServiceBuildingGlobalId,
            static (helpId, serviceIndex, serviceBuildingGlobalId) => new HelpTownServiceCommand(helpId, serviceIndex, serviceBuildingGlobalId)
        )
    );
}
