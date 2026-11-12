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
    /// Initializes a new instance of the <see cref="MapGameEventsServerCommand"/> record. Bundles a batch of Valley events for one Valley game.
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
    public static CommandCodec<MapGameEventsServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.OptionalLongId,
            static command => command.UnknownLongId0,
            Codecs.OptionalLongId,
            static command => command.MapGameId,
            Codecs.Memory(MapGameEvent.Codec, CollectionCount.AtMost(MaximumEventCount)),
            static command => command.Events,
            static (unknownLongId0, mapGameId, events) => new MapGameEventsServerCommand(unknownLongId0, mapGameId, events)),
        CommandLayout.MetadataFirst);

    /// <summary>
    /// Gets what happened in the Valley since the last batch, in order: pawn moves, tasks taken and finished, emojis placed and so on.
    /// </summary>
    /// <value>A copy of the events given to the constructor, oldest first, at most <see cref="MaximumEventCount"/>; empty when the batch carries none.</value>
    public ReadOnlyMemory<MapGameEvent> Events { get; }

    /// <summary>
    /// Gets the map-game session id.
    /// </summary>
    /// <value>The id of the Valley game the events apply to, sent second; <see langword="null"/> when the wire marks it absent.</value>
    public LongId? MapGameId { get; }

    /// <summary>
    /// Gets the optional id sent before the Valley game id; which object it names is not identified.
    /// </summary>
    /// <value>An optional 64-bit id at wire position 0, kept so the command re-encodes unchanged; <see langword="null"/> when the wire marks it absent.</value>
    [JsonPropertyName("UnknownLongId0")]
    public LongId? UnknownLongId0 { get; }
}
