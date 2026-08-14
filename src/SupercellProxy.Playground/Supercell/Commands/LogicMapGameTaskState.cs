using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// One polymorphic state inside a map-game task. Its payload type comes from the live TaskType CSV field.
/// </summary>
public sealed record LogicMapGameTaskState(
    int TaskGlobalId,
    LogicLong? Unknown0,
    bool UnknownBoolean0,
    LogicMapGameTaskStatePayload Payload,
    bool UnknownBoolean1)
{
    private const string TaskTypeFieldName = "TaskType";

    internal static LogicMapGameTaskState Decode(SupercellStream stream, ILogicCommandDataResolver? dataResolver)
    {
        var taskGlobalId = stream.ReadVarInt();
        var unknown0 = LogicMapGameWire.ReadOptionalLogicLong(stream);
        var unknownBoolean0 = stream.ReadBoolean();

        if (dataResolver is null)
            throw new NotSupportedException("Map-game task-state decoding requires the live native data-table resolver.");

        if (!dataResolver.TryResolveString(taskGlobalId, TaskTypeFieldName, out var taskType))
            throw new InvalidDataException($"Map-game task data reference {taskGlobalId} has no resolvable {TaskTypeFieldName} field.");

        var payload = LogicMapGameTaskStatePayload.Decode(taskType, stream);
        var unknownBoolean1 = stream.ReadBoolean();
        return new LogicMapGameTaskState(taskGlobalId, unknown0, unknownBoolean0, payload, unknownBoolean1);
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(TaskGlobalId);
        LogicMapGameWire.WriteOptionalLogicLong(stream, Unknown0);
        stream.WriteBoolean(UnknownBoolean0);
        Payload.Encode(stream);
        stream.WriteBoolean(UnknownBoolean1);
    }
}
