using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Serverbound command recording that the player opened an event board event's screen, which clears its first-open marker.
/// </summary>
/// <param name="EventId">Event board event whose screen was opened.</param>
public sealed record MarkChronosEventUserInterfaceOpenedCommand([property: JsonPropertyName("EventId")] int EventId) : CodecCommand<MarkChronosEventUserInterfaceOpenedCommand>, ICommandCodecProvider<MarkChronosEventUserInterfaceOpenedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkChronosEventUserInterfaceOpenedCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new MarkChronosEventUserInterfaceOpenedCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
