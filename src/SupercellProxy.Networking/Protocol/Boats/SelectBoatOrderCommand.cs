using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Serverbound command choosing where the arrived boat sails next: the player picks one of the offered destinations, and its order
/// becomes the crates to load. Only valid while the boat is waiting for a destination choice.
/// </summary>
/// <param name="OrderIndex">Index of the chosen destination among those the boat offers.</param>
public sealed record SelectBoatOrderCommand(int OrderIndex) : CodecCommand<SelectBoatOrderCommand>, ICommandCodecProvider<SelectBoatOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectBoatOrderCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.OrderIndex, static orderIndex => new SelectBoatOrderCommand(orderIndex)),
        CommandLayout.MetadataFirst
    );
}
