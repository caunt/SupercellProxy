using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports a derby request result whose native effects are presentation callbacks.</summary>
public sealed record DerbyNotificationServerCommand(int Status, LongId NeighborhoodId, LongId? InstanceId, int Value0, int Value1, int NotificationKind) : CodecServerCommand<DerbyNotificationServerCommand>, ICommandCodecProvider<DerbyNotificationServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DerbyNotificationServerCommand> Codec { get; } = Codecs.Command(
        type: 231,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Status,
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.OptionalLongId,
            static command => command.InstanceId,
            Codecs.VarInt,
            static command => command.Value0,
            Codecs.VarInt,
            static command => command.Value1,
            Codecs.VarInt,
            static command => command.NotificationKind,
            static (status, neighborhoodId, instanceId, value0, value1, notificationKind) => new DerbyNotificationServerCommand(status, neighborhoodId, instanceId, value0, value1, notificationKind)
        )
    );
}
