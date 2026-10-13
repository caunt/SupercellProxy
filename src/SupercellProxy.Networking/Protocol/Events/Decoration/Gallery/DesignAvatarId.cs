using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Gallery;

/// <summary>The design avatar id.</summary>
/// <param name="High">The high.</param>
/// <param name="Low">The low.</param>
public sealed record DesignAvatarId(int High, int Low) : ICodecProvider<DesignAvatarId>
{
    /// <inheritdoc />
    public static ICodec<DesignAvatarId> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static value => value.High,
        Codecs.Int32,
        static value => value.Low,
        static (high, low) => new DesignAvatarId(high, low)
    );
}
