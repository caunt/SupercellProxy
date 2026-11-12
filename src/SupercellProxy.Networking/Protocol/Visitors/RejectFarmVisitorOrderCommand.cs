using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>Declines a waiting farm visitor's goods offer.</summary>
/// <param name="VisitorGlobalId">Game object id of the visitor on the player's own farm, written as a VarInt.</param>
public sealed record RejectFarmVisitorOrderCommand(int VisitorGlobalId) : CodecCommand<RejectFarmVisitorOrderCommand>, ICommandCodecProvider<RejectFarmVisitorOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RejectFarmVisitorOrderCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.VisitorGlobalId,
        static visitorGlobalId => new RejectFarmVisitorOrderCommand(visitorGlobalId)));
}
