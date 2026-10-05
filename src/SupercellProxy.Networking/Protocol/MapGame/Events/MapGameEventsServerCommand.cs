using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>
/// <para>Server command 274 containing the native map-game event stream.</para>
/// </summary>
public sealed record MapGameEventsServerCommand : ServerCommand
{
    /// <summary>
    /// Defines the <c language="csharp">CommandType</c> value.
    /// </summary>
    public const int CommandType = 274;

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

    /// <summary>
    /// Gets the <c language="csharp">Events</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameEvent> Events { get; }

    /// <summary>
    /// Gets the map-game session id.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId1")]
    public LongId? MapGameId { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId0</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId0")]
    public LongId? UnknownLongId0 { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameEventsServerCommand Decode(MessageStream stream, CommandEnvironment environment, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);

        LongId? unknownLongId0 = MapGameFieldCodec.ReadOptionalLongId(stream);
        LongId? mapGameId = MapGameFieldCodec.ReadOptionalLongId(stream);
        int eventCount = stream.ReadVarInt();

        if (uint.CreateTruncating(eventCount) > MaximumEventCount)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid map-game event count: {eventCount}."));

        MapGameEvent[] events = new MapGameEvent[eventCount];

        for (int index = 0; index < events.Length; index++)
            events[index] = MapGameEvent.Decode(stream, dataResolver);

        return new MapGameEventsServerCommand(unknownLongId0, mapGameId, events);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        MapGameFieldCodec.WriteOptionalLongId(stream, UnknownLongId0);
        MapGameFieldCodec.WriteOptionalLongId(stream, MapGameId);
        stream.WriteVarInt(Events.Length);

        foreach (MapGameEvent mapGameEvent in Events.Span)
            mapGameEvent.Encode(stream);
    }
}
