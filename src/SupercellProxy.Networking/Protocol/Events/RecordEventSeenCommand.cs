using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>
/// Defines the Record Event Seen Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
/// <summary>
/// Defines the Context0 contract.
/// </summary>
/// <summary>
/// Defines the Context1 contract.
/// </summary>
public sealed record RecordEventSeenCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId, int Context0, int Context1) : CodecCommand<RecordEventSeenCommand>, ICommandCodecProvider<RecordEventSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RecordEventSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Int32,
            static command => command.EventId,
            Codecs.Int32,
            static command => command.Context0,
            Codecs.Int32,
            static command => command.Context1,
            static (id, context0, context1) => new RecordEventSeenCommand(id, context0, context1)
        )
    );
}
