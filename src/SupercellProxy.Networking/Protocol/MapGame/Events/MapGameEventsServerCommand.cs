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
    public MapGameEventsServerCommand(
        LongIdentifier? unknownLongIdentifier0,
        LongIdentifier? unknownLongIdentifier1,
        ReadOnlyMemory<MapGameEvent> events,
        int serverCommandIdentifier,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandIdentifier, executionPhaseCounter, debugData0, debugData1)
    {
        if (events.Length > MaximumEventCount)
            throw new InvalidDataException($"Invalid map-game event count: {events.Length}.");

        UnknownLongIdentifier0 = unknownLongIdentifier0;
        UnknownLongIdentifier1 = unknownLongIdentifier1;
        Events = events.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Events</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameEvent> Events { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId0</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId0")]
    public LongIdentifier? UnknownLongIdentifier0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId1</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId1")]
    public LongIdentifier? UnknownLongIdentifier1 { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameEventsServerCommand Decode(MessageStream stream, CommandEnvironment environment, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);
        LongIdentifier? unknownLongIdentifier0 = MapGameFieldCodec.ReadOptionalLongIdentifier(stream);
        LongIdentifier? unknownLongIdentifier1 = MapGameFieldCodec.ReadOptionalLongIdentifier(stream);
        int eventCount = stream.ReadVariableInt();

        if (uint.CreateTruncating(eventCount) > MaximumEventCount)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid map-game event count: {eventCount}."));

        MapGameEvent[] events = new MapGameEvent[eventCount];

        for (int index = 0; index < events.Length; index++)
            events[index] = MapGameEvent.Decode(stream, dataResolver);

        return new MapGameEventsServerCommand(
            unknownLongIdentifier0,
            unknownLongIdentifier1,
            events,
            serverCommandIdentifier,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeServerCommand(stream, environment);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownLongIdentifier0);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownLongIdentifier1);
        stream.WriteVariableInt(Events.Length);

        foreach (MapGameEvent mapGameEvent in Events.Span)
            mapGameEvent.Encode(stream);
    }
}
