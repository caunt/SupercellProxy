using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports a derby season result whose native effects are presentation callbacks.</summary>
public sealed record DerbySeasonNotificationServerCommand(int Status, LongId NeighborhoodId, LongId? InstanceId, int Value) : CodecServerCommand<DerbySeasonNotificationServerCommand>, ICommandCodecProvider<DerbySeasonNotificationServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DerbySeasonNotificationServerCommand> Codec { get; } = Codecs.Command(
        type: 323,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Status,
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.OptionalLongId,
            static command => command.InstanceId,
            Codecs.VarInt,
            static command => command.Value,
            static (status, neighborhoodId, instanceId, @value) => new DerbySeasonNotificationServerCommand(status, neighborhoodId, instanceId, value)
        )
    );
}
