using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>The reviver avatar.</summary>
/// <param name="High">The high.</param>
/// <param name="Low">The low.</param>
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
