using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Defines the Mark Chronos Event Ui Opened Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
public sealed record MarkChronosEventUserInterfaceOpenedCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId) : CodecCommand<MarkChronosEventUserInterfaceOpenedCommand>, ICommandCodecProvider<MarkChronosEventUserInterfaceOpenedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkChronosEventUserInterfaceOpenedCommand> Codec { get; } = Codecs.Command(
        type: 502,
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static (eventId) => new MarkChronosEventUserInterfaceOpenedCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
