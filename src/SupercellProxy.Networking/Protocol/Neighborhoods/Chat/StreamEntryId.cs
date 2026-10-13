using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>One stream entry id entry.</summary>
/// <param name="High">The high.</param>
/// <param name="Low">The low.</param>
public sealed record StreamEntryId(int High, int Low) : ICodecProvider<StreamEntryId>
{
    /// <inheritdoc />
    public static ICodec<StreamEntryId> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static value => value.High,
        Codecs.Int32,
        static value => value.Low,
        static (high, low) => new StreamEntryId(high, low)
    );
}
