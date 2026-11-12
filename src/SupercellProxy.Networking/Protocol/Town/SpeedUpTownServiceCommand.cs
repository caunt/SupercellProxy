using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Instantly completes a running town service using premium currency.</summary>
/// <param name="ServiceIndex">Zero-based index into the building's queue of booked services, written first as a VarInt.</param>
/// <param name="ServiceBuildingGlobalId">Game object id of the town service building running the service.</param>
public sealed record SpeedUpTownServiceCommand(int ServiceIndex, int ServiceBuildingGlobalId) : CodecCommand<SpeedUpTownServiceCommand>, ICommandCodecProvider<SpeedUpTownServiceCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpTownServiceCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.ServiceIndex,
        Codecs.VarInt,
        static command => command.ServiceBuildingGlobalId,
        static (service, building) => new SpeedUpTownServiceCommand(service, building)));
}
