using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Serverbound command, new in 1.73.81, asking for a fresh truck order in place of one on the order board.</summary>
/// <param name="Mode">Kind of refresh; its values are not identified.</param>
/// <param name="OrderIndex">Position of the replaced order on the board.</param>
public sealed record RefreshTruckOrderCommand(int Mode, int OrderIndex) : CodecCommand<RefreshTruckOrderCommand>, ICommandCodecProvider<RefreshTruckOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RefreshTruckOrderCommand> Codec { get; } = Codecs.Command(
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
