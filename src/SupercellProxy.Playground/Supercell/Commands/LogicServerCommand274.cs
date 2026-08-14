using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Server command 274 containing the native map-game event stream.
/// </summary>
public sealed record LogicServerCommand274 : LogicServerCommand
{
    public const int CommandType = 274;
    public const int MaxEventCount = 1024;

    public LogicServerCommand274(
        LogicLong? unknownLogicLong0,
        LogicLong? unknownLogicLong1,
        ReadOnlyMemory<LogicMapGameEvent> events,
        int serverCommandId,
        int executeSubTick = -1,
        LogicCommandData? debugData0 = null,
        LogicCommandData? debugData1 = null)
        : base(serverCommandId, executeSubTick, debugData0, debugData1)
    {
        if (events.Length > MaxEventCount)
            throw new InvalidDataException($"Invalid map-game event count: {events.Length}.");

        UnknownLogicLong0 = unknownLogicLong0;
        UnknownLogicLong1 = unknownLogicLong1;
        Events = events.ToArray();
    }

    public override int Type => CommandType;
    public LogicLong? UnknownLogicLong0 { get; }
    public LogicLong? UnknownLogicLong1 { get; }
    public ReadOnlyMemory<LogicMapGameEvent> Events { get; }

    internal static LogicServerCommand274 Decode(
        SupercellStream stream,
        LogicEnvironment environment,
        ILogicCommandDataResolver? dataResolver)
    {
        var commandFields = DecodeLogicServerCommand(stream, environment);
        var unknownLogicLong0 = LogicMapGameWire.ReadOptionalLogicLong(stream);
        var unknownLogicLong1 = LogicMapGameWire.ReadOptionalLogicLong(stream);
        var eventCount = stream.ReadVarInt();

        if ((uint)eventCount > MaxEventCount)
            throw new InvalidDataException($"Invalid map-game event count: {eventCount}.");

        var events = new LogicMapGameEvent[eventCount];

        for (var i = 0; i < events.Length; i++)
            events[i] = LogicMapGameEvent.Decode(stream, dataResolver);

        return new LogicServerCommand274(
            unknownLogicLong0,
            unknownLogicLong1,
            events,
            commandFields.ServerCommandId,
            commandFields.LogicCommandFields.ExecuteSubTick,
            commandFields.LogicCommandFields.DebugData0,
            commandFields.LogicCommandFields.DebugData1);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        EncodeLogicServerCommand(stream, environment);
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownLogicLong0);
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownLogicLong1);
        stream.WriteVarInt(Events.Length);

        foreach (var mapGameEvent in Events.Span)
            mapGameEvent.Encode(stream);
    }
}
