using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.TaskEvents;

/// <summary>Event types 18 and 37: chickens or sanctuary animals were delivered.</summary>
/// <param name="Type">The native event type.</param>
/// <param name="AvatarId">The contributing avatar.</param>
/// <param name="Pawn">The contributing pawn.</param>
/// <param name="DataGlobalId">The delivered animal data global id.</param>
/// <param name="Amount">The delivered amount.</param>
/// <param name="Value">The value native reads after the amount.</param>
public sealed record MapGameContributionEvent(int Type, LongId? AvatarId, MapGamePawn? Pawn, int DataGlobalId, int Amount, int Value) : MapGameEvent(Type)
{
    /// <summary>
    /// Gets the codec for the event type <paramref name="type"/>.
    /// </summary>
    internal static ICodec<MapGameContributionEvent> CodecFor(int type)
    {
        return Codecs.Composite(
            Codecs.OptionalLongId,
            static mapGameEvent => mapGameEvent.AvatarId,
            Codecs.Optional(MapGamePawn.Codec),
            static mapGameEvent => mapGameEvent.Pawn,
            Codecs.VarInt,
            static mapGameEvent => mapGameEvent.DataGlobalId,
            Codecs.VarInt,
            static mapGameEvent => mapGameEvent.Amount,
            Codecs.VarInt,
            static mapGameEvent => mapGameEvent.Value,
            (avatarId, pawn, dataGlobalId, amount, value) => new MapGameContributionEvent(type, avatarId, pawn, dataGlobalId, amount, value)
        );
    }
}
