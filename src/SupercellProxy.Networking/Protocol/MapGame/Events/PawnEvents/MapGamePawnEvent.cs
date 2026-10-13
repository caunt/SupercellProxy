using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.PawnEvents;

/// <summary>Event types 28, 30 and 31: a pawn changed.</summary>
/// <param name="Type">The native event type.</param>
/// <param name="Pawn">The changed pawn.</param>
public sealed record MapGamePawnEvent(int Type, MapGamePawn? Pawn) : MapGameEvent(Type)
{
    /// <summary>
    /// Gets the codec for the event type <paramref name="type"/>.
    /// </summary>
    internal static ICodec<MapGamePawnEvent> CodecFor(int type)
    {
        return Codecs.Composite(Codecs.Optional(MapGamePawn.Codec), static mapGameEvent => mapGameEvent.Pawn, (pawn) => new MapGamePawnEvent(type, pawn));
    }
}
