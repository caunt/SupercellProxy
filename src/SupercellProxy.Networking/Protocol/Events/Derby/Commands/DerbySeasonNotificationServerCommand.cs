using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports a derby season result whose native effects are presentation callbacks.</summary>
/// <param name="Status">The result code, the first field; its values are not yet identified.</param>
/// <param name="NeighborhoodId">The neighbourhood the season result is about.</param>
/// <param name="InstanceId">The derby instance the result is about, or <see langword="null"/> when the server sends none.</param>
/// <param name="Value">The last field, an unnamed integer whose meaning is not yet identified.</param>
public sealed record DerbySeasonNotificationServerCommand(int Status, LongId NeighborhoodId, LongId? InstanceId, int Value) : CodecServerCommand<DerbySeasonNotificationServerCommand>, ICommandCodecProvider<DerbySeasonNotificationServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DerbySeasonNotificationServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.Status,
        Codecs.LongId,
        static command => command.NeighborhoodId,
        Codecs.OptionalLongId,
        static command => command.InstanceId,
        Codecs.VarInt,
        static command => command.Value,
        static (status, neighborhoodId, instanceId, @value) => new DerbySeasonNotificationServerCommand(status, neighborhoodId, instanceId, value)));
}
