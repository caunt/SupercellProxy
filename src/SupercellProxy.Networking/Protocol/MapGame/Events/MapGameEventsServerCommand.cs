using System.Text.Json.Serialization;

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
    /// Bundles a batch of Valley events for one Valley game.
    /// </summary>
    /// <param name="unknownLongId0">Optional id sent first; which object it names is not identified.</param>
    /// <param name="mapGameId">Valley game the events belong to.</param>
    /// <param name="events">The events in the order they happened; at most <see cref="MaximumEventCount"/>.</param>
    /// <exception cref="InvalidDataException">More than <see cref="MaximumEventCount"/> events were given.</exception>
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
    /// What happened in the Valley since the last batch, in order: pawn moves, tasks taken and finished, emojis placed and so on.
    /// </summary>
    public ReadOnlyMemory<MapGameEvent> Events { get; }

    /// <summary>
    /// Gets the map-game session id.
    /// </summary>
    public LongId? MapGameId { get; }

    /// <summary>
    /// Optional id sent before the Valley game id; which object it names is not identified.
    /// </summary>
    [JsonPropertyName("UnknownLongId0")]
    public LongId? UnknownLongId0 { get; }
}
