using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Starts a selected service at a town service building.</summary>
/// <param name="ServiceIndex">Zero-based index into the building's queue of booked services, written first as a VarInt.</param>
/// <param name="ServiceBuildingGlobalId">Game object id of the town service building where the passenger is booked.</param>
public sealed record StartTownServiceCommand(int ServiceIndex, int ServiceBuildingGlobalId) : CodecCommand<StartTownServiceCommand>, ICommandCodecProvider<StartTownServiceCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartTownServiceCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.ServiceIndex,
        Codecs.VarInt,
        static command => command.ServiceBuildingGlobalId,
        static (index, building) => new StartTownServiceCommand(index, building)));
}
