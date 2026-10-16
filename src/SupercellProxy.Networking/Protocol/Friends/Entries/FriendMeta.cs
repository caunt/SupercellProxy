using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// <summary>A friend's or neighbour's friend-bar state.</summary>
/// <param name="Flags">The friend-bar flags; bit 2 is set only by the server.</param>
/// <param name="Value">The second friend-bar value.</param>
/// <param name="HomeId">The friend's home id.</param>
public sealed record FriendMeta(int Flags, int Value, LongId HomeId) : ICodecProvider<FriendMeta>
{
    /// <inheritdoc />
    public static ICodec<FriendMeta> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static meta => meta.Flags,
        Codecs.VarInt,
        static meta => meta.Value,
        Codecs.LongId,
        static meta => meta.HomeId,
        static (flags, value, homeId) => new FriendMeta(flags, value, homeId)
    );
}
