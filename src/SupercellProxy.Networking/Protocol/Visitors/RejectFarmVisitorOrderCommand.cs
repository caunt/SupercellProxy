using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>Declines a waiting farm visitor's goods offer.</summary>
public sealed record RejectFarmVisitorOrderCommand(int VisitorGlobalId) : CodecCommand<RejectFarmVisitorOrderCommand>, ICommandCodecProvider<RejectFarmVisitorOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RejectFarmVisitorOrderCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.VisitorGlobalId,
            static visitorGlobalId => new RejectFarmVisitorOrderCommand(visitorGlobalId)
        )
    );
}
