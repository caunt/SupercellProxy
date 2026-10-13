using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.TaskEvents;

/// <summary>Event type 38: escaped sanctuary animals were removed.</summary>
/// <param name="Pawn">The acting pawn.</param>
/// <param name="Tasks">The remaining escaped-animal tasks.</param>
public sealed record MapGameEscapedSanctuaryAnimalsRemovedEvent(MapGamePawn? Pawn, MapGameTaskCollection? Tasks) : MapGameEvent(MapGameEvent.EscapedSanctuaryAnimalsRemovedType), ICodecProvider<MapGameEscapedSanctuaryAnimalsRemovedEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGameEscapedSanctuaryAnimalsRemovedEvent> Codec { get; } = Codecs.Composite(
        Codecs.Optional(MapGamePawn.Codec),
        static mapGameEvent => mapGameEvent.Pawn,
        Codecs.Optional(MapGameTaskCollection.Codec),
        static mapGameEvent => mapGameEvent.Tasks,
        static (pawn, tasks) => new MapGameEscapedSanctuaryAnimalsRemovedEvent(pawn, tasks)
    );
}
