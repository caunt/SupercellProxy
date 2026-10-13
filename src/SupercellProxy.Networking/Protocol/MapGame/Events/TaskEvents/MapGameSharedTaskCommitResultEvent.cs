using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.TaskEvents;

/// <summary>Event type 29: the result of committing goods to a shared task.</summary>
/// <param name="Status">The commit status.</param>
/// <param name="Pawn">The acting pawn.</param>
/// <param name="Task">The task.</param>
/// <param name="Submission">The submitted dump-task state.</param>
public sealed record MapGameSharedTaskCommitResultEvent(int Status, MapGamePawn? Pawn, MapGameTask? Task, MapGameDumpTaskStatePayload? Submission) : MapGamePawnTaskEvent(MapGameEvent.SharedTaskCommitResultType, Pawn, Task), ICodecProvider<MapGameSharedTaskCommitResultEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGameSharedTaskCommitResultEvent> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.Status,
        Codecs.Optional(MapGamePawn.Codec),
        static mapGameEvent => mapGameEvent.Pawn,
        Codecs.Optional(MapGameTask.Codec),
        static mapGameEvent => mapGameEvent.Task,
        Codecs.Optional(MapGameDumpTaskStatePayload.Codec),
        static mapGameEvent => mapGameEvent.Submission,
        static (status, pawn, task, submission) => new MapGameSharedTaskCommitResultEvent(status, pawn, task, submission)
    );
}
