using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>Replaces the passengers of a town service building after neighborhood help.</summary>
/// <param name="OwnerHomeId">The owner home id.</param>
/// <param name="ServiceObjectGlobalId">The service object id.</param>
/// <param name="Passengers">The passengers.</param>
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
