using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Gallery;

/// <summary>Account that made a decoration design, sent as its two id words; used to name the design in gallery messages.</summary>
/// <param name="High">First (shard) word of the designer's account id.</param>
/// <param name="Low">Second (row) word of the designer's account id.</param>
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
