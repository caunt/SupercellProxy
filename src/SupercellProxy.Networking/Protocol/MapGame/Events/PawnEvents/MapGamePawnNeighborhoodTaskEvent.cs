using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.PawnEvents;

/// <summary>Event type 25: a pawn neighborhood task changed.</summary>
/// <param name="AvatarId">The pawn owner.</param>
/// <param name="Operation">The change operation.</param>
/// <param name="DataGlobalId">The neighborhood task data global id.</param>
public sealed record MapGamePawnNeighborhoodTaskEvent(LongId? AvatarId, int Operation, int DataGlobalId) : MapGameEvent(MapGameEvent.PawnNeighborhoodTaskUpdatedType), ICodecProvider<MapGamePawnNeighborhoodTaskEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGamePawnNeighborhoodTaskEvent> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static mapGameEvent => mapGameEvent.AvatarId,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.Operation,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.DataGlobalId,
        static (avatarId, operation, dataGlobalId) => new MapGamePawnNeighborhoodTaskEvent(avatarId, operation, dataGlobalId)
    );
}
