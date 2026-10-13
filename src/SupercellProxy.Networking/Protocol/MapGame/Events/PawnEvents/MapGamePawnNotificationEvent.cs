using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.PawnEvents;

/// <summary>Event type 27: a pawn notification changed.</summary>
/// <param name="AvatarId">The pawn owner.</param>
/// <param name="Operation">The change operation.</param>
/// <param name="Value">The notification value.</param>
/// <param name="DataGlobalId">The notification data global id.</param>
public sealed record MapGamePawnNotificationEvent(LongId AvatarId, int Operation, int Value, int DataGlobalId) : MapGameEvent(MapGameEvent.PawnNotificationUpdatedType), ICodecProvider<MapGamePawnNotificationEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGamePawnNotificationEvent> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static mapGameEvent => mapGameEvent.AvatarId,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.Operation,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.Value,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.DataGlobalId,
        static (avatarId, operation, value, dataGlobalId) => new MapGamePawnNotificationEvent(avatarId, operation, value, dataGlobalId)
    );
}
