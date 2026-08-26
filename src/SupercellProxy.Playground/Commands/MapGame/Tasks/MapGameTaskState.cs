using System.Globalization;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>One polymorphic state inside a map-game task. Its payload type comes from the live TaskType CSV field.</para>
/// </summary>
public sealed record MapGameTaskState(
    int TaskGlobalId,
    LongId? Unknown0,
    bool UnknownBoolean0,
    MapGameTaskStatePayload Payload,
    bool UnknownBoolean1
)
{
    private const string TaskTypeFieldName = "TaskType";

    internal static MapGameTaskState Decode(
        MessageStream stream,
        ICommandDataResolver? dataResolver
    )
    {
        var taskGlobalId = stream.ReadVarInt();
        var unknown0 = MapGameWire.ReadOptionalLongId(stream);
        var unknownBoolean0 = stream.ReadBoolean();

        if (dataResolver is null)
            throw new NotSupportedException(
                "Map-game task-state decoding requires the live native data-table resolver."
            );

        if (!dataResolver.TryResolveString(taskGlobalId, TaskTypeFieldName, out var taskType))
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Map-game task data reference {taskGlobalId} has no resolvable {TaskTypeFieldName} field."
                )
            );

        var payload = MapGameTaskStatePayload.Decode(taskType, stream);
        var unknownBoolean1 = stream.ReadBoolean();
        return new MapGameTaskState(
            taskGlobalId,
            unknown0,
            unknownBoolean0,
            payload,
            unknownBoolean1
        );
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(TaskGlobalId);
        MapGameWire.WriteOptionalLongId(stream, Unknown0);
        stream.WriteBoolean(UnknownBoolean0);
        Payload.Encode(stream);
        stream.WriteBoolean(UnknownBoolean1);
    }
}
