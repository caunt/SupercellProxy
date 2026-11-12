using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports a derby request result whose native effects are presentation callbacks.</summary>
/// <param name="Status">The result code of the request, the first field; its values are not yet identified.</param>
/// <param name="NeighborhoodId">The neighbourhood the notification is about.</param>
/// <param name="InstanceId">The derby instance the notification is about, or <see langword="null"/> when the server sends none.</param>
/// <param name="Value0">The first unnamed integer of the notification, after the instance id; its meaning is not yet identified.</param>
/// <param name="Value1">The second unnamed integer of the notification; its meaning is not yet identified.</param>
/// <param name="NotificationKind">The last field, a native number selecting the kind of notification shown; its values are not yet identified.</param>
public sealed record DerbyNotificationServerCommand(int Status, LongId NeighborhoodId, LongId? InstanceId, int Value0, int Value1, int NotificationKind) : CodecServerCommand<DerbyNotificationServerCommand>, ICommandCodecProvider<DerbyNotificationServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DerbyNotificationServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
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
        static (status, neighborhoodId, instanceId, value0, value1, notificationKind) => new DerbyNotificationServerCommand(status, neighborhoodId, instanceId, value0, value1, notificationKind)));
}
