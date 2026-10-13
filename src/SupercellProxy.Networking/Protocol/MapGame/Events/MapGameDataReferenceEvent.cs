using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>Event type 40: a value followed by a data reference.</summary>
/// <param name="Value">The value native reads first.</param>
/// <param name="DataGlobalId">The referenced data global id.</param>
public sealed record MapGameDataReferenceEvent(int Value, int DataGlobalId) : MapGameEvent(MapGameEvent.DataReferenceType), ICodecProvider<MapGameDataReferenceEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGameDataReferenceEvent> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.Value,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.DataGlobalId,
        static (value, dataGlobalId) => new MapGameDataReferenceEvent(value, dataGlobalId)
    );
}
