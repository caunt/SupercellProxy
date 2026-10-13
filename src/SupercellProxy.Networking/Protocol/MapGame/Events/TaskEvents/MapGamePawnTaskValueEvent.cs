using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.TaskEvents;

/// <summary>Event types 14, 15 and 35: a pawn, a task and a value, such as a diamond cost.</summary>
/// <param name="Type">The native event type.</param>
/// <param name="Pawn">The acting pawn.</param>
/// <param name="Task">The task.</param>
/// <param name="Value">The value native reads after the task.</param>
public sealed record MapGamePawnTaskValueEvent(int Type, MapGamePawn? Pawn, MapGameTask? Task, int Value) : MapGamePawnTaskEvent(Type, Pawn, Task)
{
    /// <summary>
    /// Gets the codec for the event type <paramref name="type"/>.
    /// </summary>
    internal static new ICodec<MapGamePawnTaskValueEvent> CodecFor(int type)
    {
        return Codecs.Composite(
            Codecs.Optional(MapGamePawn.Codec),
            static mapGameEvent => mapGameEvent.Pawn,
            Codecs.Optional(MapGameTask.Codec),
            static mapGameEvent => mapGameEvent.Task,
            Codecs.VarInt,
            static mapGameEvent => mapGameEvent.Value,
            (pawn, task, value) => new MapGamePawnTaskValueEvent(type, pawn, task, value)
        );
    }
}
