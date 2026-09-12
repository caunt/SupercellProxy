using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>One polymorphic state inside a map-game task. Its payload type comes from the live TaskType CSV field.</para>
/// </summary>
public sealed record MapGameTaskState(
    [property: System.Text.Json.Serialization.JsonPropertyName("TaskGlobalId")] int TaskGlobalIdentifier,
    LongIdentifier? Unknown0,
    bool UnknownBoolean0,
    MapGameTaskStatePayload Payload,
    bool UnknownBoolean1
)
{
    private const string TaskTypeFieldName = "TaskType";

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameTaskState Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int taskGlobalIdentifier = stream.ReadVariableInt();
        LongIdentifier? unknown0 = MapGameFieldCodec.ReadOptionalLongIdentifier(stream);
        bool unknownBoolean0 = stream.ReadBoolean();

        if (dataResolver is null)
            throw new NotSupportedException(message: "Map-game task-state decoding requires the live native data-table resolver.");

        if (!dataResolver.TryResolveString(taskGlobalIdentifier, TaskTypeFieldName, out string? taskType))
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Map-game task data reference {taskGlobalIdentifier} has no resolvable {TaskTypeFieldName} field."
                )
            );
        }

        MapGameTaskStatePayload payload = MapGameTaskStatePayload.Decode(taskType, stream);
        bool unknownBoolean1 = stream.ReadBoolean();

        return new MapGameTaskState(taskGlobalIdentifier, unknown0, unknownBoolean0, payload, unknownBoolean1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(TaskGlobalIdentifier);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, Unknown0);
        stream.WriteBoolean(UnknownBoolean0);
        Payload.Encode(stream);
        stream.WriteBoolean(UnknownBoolean1);
    }
}
