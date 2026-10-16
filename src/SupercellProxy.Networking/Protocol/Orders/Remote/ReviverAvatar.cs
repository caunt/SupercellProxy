using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>Home id of the player who filled an order for its owner, sent as its two 32-bit halves.</summary>
/// <param name="High">Upper 32 bits of the home id.</param>
/// <param name="Low">Lower 32 bits of the home id.</param>
public sealed record ReviverAvatar(int High, int Low) : ICodecProvider<ReviverAvatar>
{
    /// <inheritdoc />
    public static ICodec<ReviverAvatar> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static value => value.High,
        Codecs.Int32,
        static value => value.Low,
        static (high, low) => new ReviverAvatar(high, low)
    );
}
