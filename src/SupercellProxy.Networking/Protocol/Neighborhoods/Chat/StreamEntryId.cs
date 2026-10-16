using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Identifier of a neighbourhood stream entry, sent as its two 32-bit halves.</summary>
/// <param name="High">Upper 32 bits of the identifier.</param>
/// <param name="Low">Lower 32 bits of the identifier.</param>
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
