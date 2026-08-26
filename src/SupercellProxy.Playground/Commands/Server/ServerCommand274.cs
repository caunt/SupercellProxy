using System.Globalization;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Server command 274 containing the native map-game event stream.</para>
/// </summary>
public sealed record ServerCommand274 : ServerCommand
{
    /// <summary>
    /// Defines the <c>CommandType</c> value.
    /// </summary>
    public const int CommandType = 274;

    /// <summary>
    /// Defines the <c>MaxEventCount</c> value.
    /// </summary>
    public const int MaxEventCount = 1024;

    /// <summary>
    /// Initializes a new <see cref="ServerCommand274"/> instance.
    /// </summary>
    public ServerCommand274(
        LongId? unknownLongId0,
        LongId? unknownLongId1,
        ReadOnlyMemory<MapGameEvent> events,
        int serverCommandId,
        int executeSubTick = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandId, executeSubTick, debugData0, debugData1)
    {
        if (events.Length > MaxEventCount)
            throw new InvalidDataException($"Invalid map-game event count: {events.Length}.");

        UnknownLongId0 = unknownLongId0;
        UnknownLongId1 = unknownLongId1;
        Events = events.ToArray();
    }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the <c>UnknownLongId0</c> value.
    /// </summary>
    public LongId? UnknownLongId0 { get; }

    /// <summary>
    /// Gets the <c>UnknownLongId1</c> value.
    /// </summary>
    public LongId? UnknownLongId1 { get; }

    /// <summary>
    /// Gets the <c>Events</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameEvent> Events { get; }

    internal static ServerCommand274 Decode(
        MessageStream stream,
        CommandEnvironment environment,
        ICommandDataResolver? dataResolver
    )
    {
        var commandFields = DecodeServerCommand(stream, environment);
        var unknownLongId0 = MapGameWire.ReadOptionalLongId(stream);
        var unknownLongId1 = MapGameWire.ReadOptionalLongId(stream);
        var eventCount = stream.ReadVarInt();

        if (uint.CreateTruncating(eventCount) > MaxEventCount)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid map-game event count: {eventCount}."
                )
            );

        var events = new MapGameEvent[eventCount];

        for (var i = 0; i < events.Length; i++)
            events[i] = MapGameEvent.Decode(stream, dataResolver);

        return new ServerCommand274(
            unknownLongId0,
            unknownLongId1,
            events,
            commandFields.ServerCommandId,
            commandFields.CommandFields.ExecuteSubTick,
            commandFields.CommandFields.DebugData0,
            commandFields.CommandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeServerCommand(stream, environment);
        MapGameWire.WriteOptionalLongId(stream, UnknownLongId0);
        MapGameWire.WriteOptionalLongId(stream, UnknownLongId1);
        stream.WriteVarInt(Events.Length);

        foreach (var mapGameEvent in Events.Span)
            mapGameEvent.Encode(stream);
    }
}
