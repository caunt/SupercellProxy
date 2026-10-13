using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Starts a selected service at a town service building.</summary>
public sealed record StartTownServiceCommand(int ServiceIndex, int ServiceBuildingGlobalId) : CodecCommand<StartTownServiceCommand>, ICommandCodecProvider<StartTownServiceCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartTownServiceCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ServiceIndex,
            Codecs.VarInt,
            static command => command.ServiceBuildingGlobalId,
            static (index, building) => new StartTownServiceCommand(index, building)
        )
    );
}
