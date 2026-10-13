using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>Accepts a waiting farm visitor's offer for its requested goods.</summary>
public sealed record FulfillFarmVisitorOrderCommand(int VisitorGlobalId) : CodecCommand<FulfillFarmVisitorOrderCommand>, ICommandCodecProvider<FulfillFarmVisitorOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FulfillFarmVisitorOrderCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.VisitorGlobalId,
            static (visitorGlobalId) => new FulfillFarmVisitorOrderCommand(visitorGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
