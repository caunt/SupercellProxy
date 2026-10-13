using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>Event type 1: the server replaces the whole map-game state.</summary>
/// <param name="MapGameId">The map game the state belongs to.</param>
/// <param name="EventId">The map-game live event id.</param>
/// <param name="State">The replacement state.</param>
/// <param name="Pawn">The avatar pawn.</param>
/// <param name="Value">The trailing flag native reads after the pawn.</param>
public sealed record MapGameStateSynchronizationEvent(LongId? MapGameId, int EventId, MapGameState? State, MapGamePawn? Pawn, bool Value) : MapGameEvent(MapGameEvent.StateSynchronizationType), ICodecProvider<MapGameStateSynchronizationEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGameStateSynchronizationEvent> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static mapGameEvent => mapGameEvent.MapGameId,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.EventId,
        Codecs.Optional(MapGameState.Codec),
        static mapGameEvent => mapGameEvent.State,
        Codecs.Optional(MapGamePawn.Codec),
        static mapGameEvent => mapGameEvent.Pawn,
        Codecs.Boolean,
        static mapGameEvent => mapGameEvent.Value,
        static (mapGameId, eventId, state, pawn, value) => new MapGameStateSynchronizationEvent(mapGameId, eventId, state, pawn, value)
    );
}
