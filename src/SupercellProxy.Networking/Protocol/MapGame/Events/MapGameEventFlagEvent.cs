using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>Event type 26: a live event flag changed.</summary>
/// <param name="EventId">The live event id.</param>
/// <param name="Value">The new flag value.</param>
public sealed record MapGameEventFlagEvent(int EventId, bool Value) : MapGameEvent(MapGameEvent.EventFlagUpdatedType), ICodecProvider<MapGameEventFlagEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGameEventFlagEvent> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.EventId,
        Codecs.Boolean,
        static mapGameEvent => mapGameEvent.Value,
        static (eventId, value) => new MapGameEventFlagEvent(eventId, value)
    );
}
