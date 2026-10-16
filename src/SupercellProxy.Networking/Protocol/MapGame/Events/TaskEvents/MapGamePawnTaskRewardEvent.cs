using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.TaskEvents;

/// <summary>Event types 17, 21 and 32: a pawn, a task and the optional reward values.</summary>
/// <param name="Type">Native event number: 17, 21 or 32.</param>
/// <param name="Pawn">The acting pawn.</param>
/// <param name="Task">Valley task the reward comes from.</param>
/// <param name="Values">The reward values, when present.</param>
public sealed record MapGamePawnTaskRewardEvent(int Type, MapGamePawn? Pawn, MapGameTask? Task, ReadOnlyMemory<int>? Values) : MapGamePawnTaskEvent(Type, Pawn, Task)
{
    /// <summary>
    /// Gets the codec for the event type <paramref name="type"/>.
    /// </summary>
    internal static new ICodec<MapGamePawnTaskRewardEvent> CodecFor(int type)
    {
        return Codecs.Composite(
            Codecs.Optional(MapGamePawn.Codec),
            static mapGameEvent => mapGameEvent.Pawn,
            Codecs.Optional(MapGameTask.Codec),
            static mapGameEvent => mapGameEvent.Task,
            Codecs.OptionalValue(Codecs.Memory(Codecs.VarInt)),
            static mapGameEvent => mapGameEvent.Values,
            (pawn, task, values) => new MapGamePawnTaskRewardEvent(type, pawn, task, values)
        );
    }
}
