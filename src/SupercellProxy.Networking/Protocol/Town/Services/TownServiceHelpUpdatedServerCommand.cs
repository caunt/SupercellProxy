using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>Replaces the passengers of a town service building after neighborhood help.</summary>
/// <param name="OwnerHomeId">Home whose town the building is in.</param>
/// <param name="ServiceObjectGlobalId">Global id of the service building's game object.</param>
/// <param name="Passengers">The building's passengers after the help, replacing the ones it had.</param>
public sealed record TownServiceHelpUpdatedServerCommand(LongId OwnerHomeId, int ServiceObjectGlobalId, ReadOnlyMemory<TownServicePassenger> Passengers) : CodecServerCommand<TownServiceHelpUpdatedServerCommand>, ICommandCodecProvider<TownServiceHelpUpdatedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<TownServiceHelpUpdatedServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.VarInt,
            static command => command.ServiceObjectGlobalId,
            Codecs.Memory(TownServicePassenger.Codec),
            static command => command.Passengers,
            static (ownerHomeId, serviceObjectGlobalId, passengers) => new TownServiceHelpUpdatedServerCommand(ownerHomeId, serviceObjectGlobalId, passengers)
        )
    );
}
