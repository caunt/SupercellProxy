using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>Event type 39: a value followed by an optional event profile.</summary>
/// <param name="Value">The value native reads first.</param>
/// <param name="Profile">The event profile.</param>
public sealed record MapGameProfileEvent(int Value, MapGameEventProfileData? Profile) : MapGameEvent(MapGameEvent.ProfileType), ICodecProvider<MapGameProfileEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGameProfileEvent> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.Value,
        Codecs.Optional(MapGameEventProfileData.Codec),
        static mapGameEvent => mapGameEvent.Profile,
        static (value, profile) => new MapGameProfileEvent(value, profile)
    );
}
