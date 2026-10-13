using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>Thanks the player who helped a town service.</summary>
/// <param name="ServiceIndex">Gets the service index inside the building.</param>
/// <param name="ServiceBuildingGlobalId">Gets the service building game object id.</param>
public sealed record ThankTownServiceHelperCommand(int ServiceIndex, int ServiceBuildingGlobalId) : CodecCommand<ThankTownServiceHelperCommand>, ICommandCodecProvider<ThankTownServiceHelperCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ThankTownServiceHelperCommand> Codec { get; } = Codecs.Command(
        type: 254,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ServiceIndex,
            Codecs.VarInt,
            static command => command.ServiceBuildingGlobalId,
            static (serviceIndex, serviceBuildingGlobalId) => new ThankTownServiceHelperCommand(serviceIndex, serviceBuildingGlobalId)
        )
    );
}
