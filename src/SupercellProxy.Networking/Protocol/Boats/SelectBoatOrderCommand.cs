using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Select Boat Order Command contract.
/// </summary>
/// <summary>
/// Defines the Order Index contract.
/// </summary>
public sealed record SelectBoatOrderCommand(int OrderIndex) : CodecCommand<SelectBoatOrderCommand>, ICommandCodecProvider<SelectBoatOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectBoatOrderCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.OrderIndex, static (orderIndex) => new SelectBoatOrderCommand(orderIndex)),
        CommandLayout.MetadataFirst
    );
}
