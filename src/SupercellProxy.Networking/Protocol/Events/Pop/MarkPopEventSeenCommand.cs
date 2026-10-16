using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Pop;

/// <summary>Records the last seen pop event.</summary>
/// <param name="EventId">The seen event id.</param>
public sealed record MarkPopEventSeenCommand(int EventId) : CodecCommand<MarkPopEventSeenCommand>, ICommandCodecProvider<MarkPopEventSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkPopEventSeenCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new MarkPopEventSeenCommand(eventId)));
}
