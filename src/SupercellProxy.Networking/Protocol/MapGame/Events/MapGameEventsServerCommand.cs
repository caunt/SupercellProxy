using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>
/// <para>Server command 274 containing the native map-game event stream.</para>
/// </summary>
public sealed record MapGameEventsServerCommand : CodecServerCommand<MapGameEventsServerCommand>, ICommandCodecProvider<MapGameEventsServerCommand>
{
    /// <summary>
    /// Defines the <c language="csharp">MaxEventCount</c> value.
    /// </summary>
    public const int MaximumEventCount = 1024;

    /// <summary>
    /// Initializes a new <see cref="MapGameEventsServerCommand"/> instance.
    /// </summary>
    public MapGameEventsServerCommand(LongId? unknownLongId0, LongId? mapGameId, ReadOnlyMemory<MapGameEvent> events)
    {
        if (events.Length > MaximumEventCount)
            throw new InvalidDataException($"Invalid map-game event count: {events.Length}.");

        UnknownLongId0 = unknownLongId0;
        MapGameId = mapGameId;
        Events = events.ToArray();
    }

    /// <inheritdoc />
    public static CommandCodec<MapGameEventsServerCommand> Codec { get; } = Codecs.Command(
        type: 274,
        Codecs.Composite(
            Codecs.OptionalLongId,
            static command => command.UnknownLongId0,
            Codecs.OptionalLongId,
            static command => command.MapGameId,
            Codecs.Memory(MapGameEvent.Codec, CollectionCount.AtMost(MaximumEventCount)),
            static command => command.Events,
            static (unknownLongId0, mapGameId, events) => new MapGameEventsServerCommand(unknownLongId0, mapGameId, events)
        ),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Gets the <c language="csharp">Events</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameEvent> Events { get; }

    /// <summary>
    /// Gets the map-game session id.
    /// </summary>
    public LongId? MapGameId { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId0</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId0")]
    public LongId? UnknownLongId0 { get; }
}
