using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>One polymorphic state inside a map-game task. Its payload type comes from the live TaskType CSV field.</para>
/// </summary>
public sealed record MapGameTaskState(
    [property: System.Text.Json.Serialization.JsonPropertyName("TaskGlobalId")] int TaskGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown0")] LongId? AvatarId,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownBoolean0")] bool Completed,
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
        int taskGlobalId = stream.ReadVarInt();
        LongId? avatarId = MapGameFieldCodec.ReadOptionalLongId(stream);
        bool completed = stream.ReadBoolean();

        if (dataResolver is null)
            throw new NotSupportedException(message: "Map-game task-state decoding requires the live native data-table resolver.");

        if (!dataResolver.TryResolveString(taskGlobalId, TaskTypeFieldName, out string? taskType))
        {
            throw new InvalidDataException(
                string.Create(CultureInfo.InvariantCulture, $"Map-game task data reference {taskGlobalId} has no resolvable {TaskTypeFieldName} field.")
            );
        }

        MapGameTaskStatePayload payload = MapGameTaskStatePayload.Decode(taskType, stream);
        bool unknownBoolean1 = stream.ReadBoolean();

        return new MapGameTaskState(taskGlobalId, avatarId, completed, payload, unknownBoolean1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(TaskGlobalId);
        MapGameFieldCodec.WriteOptionalLongId(stream, AvatarId);
        stream.WriteBoolean(Completed);
        Payload.Encode(stream);
        stream.WriteBoolean(UnknownBoolean1);
    }
}
