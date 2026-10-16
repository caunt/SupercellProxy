using System.Globalization;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>One polymorphic state inside a map-game task. Its payload type comes from the live TaskType CSV field.</para>
/// <para>Native saves it with the keys <c>TaskDataGlobalId</c>, <c>PawnAvatarId</c>, <c>Complete</c>, the payload's key and <c>SocialRewardCollected</c>.</para>
/// </summary>
public sealed record MapGameTaskState(
    [property: JsonPropertyName("TaskGlobalId")] int TaskGlobalId,
    LongId? AvatarId,
    bool Completed,
    MapGameTaskStatePayload Payload,
    bool SocialRewardCollected
) : ICodecProvider<MapGameTaskState>
{
    private const string TaskTypeFieldName = "TaskType";

    /// <inheritdoc />
    /// <remarks>The payload layout follows the task data's <c>TaskType</c>, resolved through <see cref="MessageStream.CommandDataResolver"/>.</remarks>
    public static ICodec<MapGameTaskState> Codec { get; } = Codecs.Create(
        static stream =>
        {
            int taskGlobalId = stream.ReadVarInt();
            LongId? avatarId = stream.ReadOptionalLongId();
            bool completed = stream.ReadBoolean();

            ICommandDataResolver dataResolver =
                stream.CommandDataResolver
                ?? throw new NotSupportedException(message: "Map-game task-state decoding requires the live native data-table resolver.");

            if (!dataResolver.TryResolveString(taskGlobalId, TaskTypeFieldName, out string? taskType))
            {
                throw new InvalidDataException(
                    string.Create(CultureInfo.InvariantCulture, $"Map-game task data reference {taskGlobalId} has no resolvable {TaskTypeFieldName} field.")
                );
            }

            MapGameTaskStatePayload payload = MapGameTaskStatePayload.For(taskType).Read(stream);

            return new MapGameTaskState(taskGlobalId, avatarId, completed, payload, stream.ReadBoolean());
        },
        static (stream, state) =>
        {
            stream.WriteVarInt(state.TaskGlobalId);
            stream.WriteOptionalLongId(state.AvatarId);
            stream.WriteBoolean(state.Completed);
            MapGameTaskStatePayload.For(state.Payload.TaskType).Write(stream, state.Payload);
            stream.WriteBoolean(state.SocialRewardCollected);
        }
    );
}
