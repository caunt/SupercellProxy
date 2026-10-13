using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>Event type 5: an id followed by two values.</summary>
/// <param name="Id">The id native reads first.</param>
/// <param name="Value0">The first value.</param>
/// <param name="Value1">The second value.</param>
public sealed record MapGameIdValuesEvent(LongId Id, int Value0, int Value1) : MapGameEvent(MapGameEvent.IdValuesType), ICodecProvider<MapGameIdValuesEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGameIdValuesEvent> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static mapGameEvent => mapGameEvent.Id,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.Value0,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.Value1,
        static (id, value0, value1) => new MapGameIdValuesEvent(id, value0, value1)
    );
}
