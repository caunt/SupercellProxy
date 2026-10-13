using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Refreshes a truck order.</summary>
/// <param name="Mode">The mode.</param>
/// <param name="OrderIndex">The order index.</param>
public sealed record RefreshTruckOrderCommand(int Mode, int OrderIndex) : CodecCommand<RefreshTruckOrderCommand>, ICommandCodecProvider<RefreshTruckOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RefreshTruckOrderCommand> Codec { get; } = Codecs.Command(
        type: 2694,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Mode,
            Codecs.VarInt,
            static command => command.OrderIndex,
            static (mode, orderIndex) => new RefreshTruckOrderCommand(mode, orderIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
