using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>Replaces the truck orders of another home with the server's copy.</summary>
/// <param name="OwnerHomeId">The owner home id.</param>
/// <param name="Orders">The orders.</param>
public sealed record RemoteOrderUpdatesServerCommand(LongId OwnerHomeId, ReadOnlyMemory<RemoteOrder> Orders) : CodecServerCommand<RemoteOrderUpdatesServerCommand>, ICommandCodecProvider<RemoteOrderUpdatesServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RemoteOrderUpdatesServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.Memory(RemoteOrder.Codec),
            static command => command.Orders,
            static (ownerHomeId, orders) => new RemoteOrderUpdatesServerCommand(ownerHomeId, orders)
        )
    );
}
