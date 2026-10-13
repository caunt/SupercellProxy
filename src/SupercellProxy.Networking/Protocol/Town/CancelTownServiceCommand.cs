using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Cancels an unstarted town service and sends its passenger toward the train.</summary>
public sealed record CancelTownServiceCommand(int ServiceIndex, int ServiceBuildingGlobalId) : CodecCommand<CancelTownServiceCommand>, ICommandCodecProvider<CancelTownServiceCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CancelTownServiceCommand> Codec { get; } = Codecs.Command(
        type: 147,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ServiceIndex,
            Codecs.VarInt,
            static command => command.ServiceBuildingGlobalId,
            static (service, building) => new CancelTownServiceCommand(service, building)
        )
    );
}
