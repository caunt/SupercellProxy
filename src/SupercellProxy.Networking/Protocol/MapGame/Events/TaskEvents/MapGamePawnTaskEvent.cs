using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.TaskEvents;

/// <summary>Map-game events that carry a pawn and a task.</summary>
/// <param name="Type">The native event type.</param>
/// <param name="Pawn">The acting pawn.</param>
/// <param name="Task">The task.</param>
public record MapGamePawnTaskEvent(int Type, MapGamePawn? Pawn, MapGameTask? Task) : MapGameEvent(Type)
{
    /// <summary>
    /// Gets the codec for the event type <paramref name="type"/>.
    /// </summary>
    internal static ICodec<MapGamePawnTaskEvent> CodecFor(int type)
    {
        return Codecs.Composite(
            Codecs.Optional(MapGamePawn.Codec),
            static mapGameEvent => mapGameEvent.Pawn,
            Codecs.Optional(MapGameTask.Codec),
            static mapGameEvent => mapGameEvent.Task,
            (pawn, task) => new MapGamePawnTaskEvent(type, pawn, task)
        );
    }
}
