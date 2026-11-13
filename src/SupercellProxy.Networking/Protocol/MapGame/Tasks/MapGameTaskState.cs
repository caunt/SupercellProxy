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
/// <param name="TaskGlobalId">Global id of the Valley task row; its <c>TaskType</c> column selects the payload layout, and decoding fails when the row has none.</param>
/// <param name="AvatarId">Account of the player this state belongs to; <see langword="null"/> when the slot has no player, as after a player leaves a shared task unfinished.</param>
/// <param name="Completed">Whether this player has finished their part of the task.</param>
/// <param name="Payload">The task-type specific part, such as the goods a dump task needs or the collectors of a chicken task.</param>
/// <param name="SocialRewardCollected">Whether this player has already taken the reward of a completed social task.</param>
public sealed record MapGameTaskState([property: JsonPropertyName("TaskGlobalId")] int TaskGlobalId,
    LongId? AvatarId,
    bool Completed,
    MapGameTaskStatePayload Payload,
    bool SocialRewardCollected) : ICodecProvider<MapGameTaskState>
{
    private const string TaskTypeFieldName = "TaskType";

    /// <inheritdoc />
    /// <remarks>The payload layout follows the task data's <c>TaskType</c>, resolved through <see cref="MessageChannel.CommandDataResolver"/>.</remarks>
    public static ICodec<MapGameTaskState> Codec { get; } = Codecs.Create(static stream =>
    {
        int taskGlobalId = stream.ReadVarInt();
        LongId? avatarId = stream.ReadOptionalLongId();
        bool completed = stream.ReadBoolean();

        ICommandDataResolver dataResolver =
            stream.CommandDataResolver
            ?? throw new NotSupportedException(message: "Map-game task-state decoding requires the live native data-table resolver.");

        if (!dataResolver.TryResolveString(taskGlobalId, TaskTypeFieldName, out string? taskType))
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Map-game task data reference {taskGlobalId} has no resolvable {TaskTypeFieldName} field."));

        MapGameTaskStatePayload payload = MapGameTaskStatePayload.For(taskType).Read(stream);

        return new MapGameTaskState(taskGlobalId, avatarId, completed, payload, stream.ReadBoolean());
    },
        static (stream, state) =>
        {
            // The fixed part every task state starts with.
            stream.WriteVarInt(state.TaskGlobalId);
            stream.WriteOptionalLongId(state.AvatarId);
            stream.WriteBoolean(state.Completed);

            // The payload layout depends on the task type, and the social reward flag closes the record.
            MapGameTaskStatePayload.For(state.Payload.TaskType).Write(stream, state.Payload);
            stream.WriteBoolean(state.SocialRewardCollected);
        });
}
