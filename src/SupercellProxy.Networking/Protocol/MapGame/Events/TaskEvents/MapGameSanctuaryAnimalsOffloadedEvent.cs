using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events.TaskEvents;

/// <summary>Event type 33: sanctuary animals were offloaded.</summary>
/// <param name="DiamondCost">Diamonds the player paid to offload the animals.</param>
/// <param name="Pawn">The acting pawn.</param>
public sealed record MapGameSanctuaryAnimalsOffloadedEvent(int DiamondCost, MapGamePawn? Pawn) : MapGameEvent(MapGameEvent.SanctuaryAnimalsOffloadedType), ICodecProvider<MapGameSanctuaryAnimalsOffloadedEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGameSanctuaryAnimalsOffloadedEvent> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.DiamondCost,
        Codecs.Optional(MapGamePawn.Codec),
        static mapGameEvent => mapGameEvent.Pawn,
        static (diamondCost, pawn) => new MapGameSanctuaryAnimalsOffloadedEvent(diamondCost, pawn)
    );
}
