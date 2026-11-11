using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// The Release17286-family first LoginOkMessage block. The analyzed current
/// Clash Royale 20963 block also carries a VarInt, required string, and an
/// additional optional string; see docs/clash-royale/loginok-20963.md.
/// </summary>
/// <param name="CompressedData">Compressed document the block opens with; what it holds is not identified. Null when absent.</param>
/// <param name="FirstFlag">Flag that follows the document; native stores it as read, and its meaning is not identified.</param>
/// <param name="SecondFlag">The second flag, which native stores inverted.</param>
public sealed record LoginOkSessionData(ReadOnlyMemory<byte>? CompressedData, bool FirstFlag, bool SecondFlag) : ICodecProvider<LoginOkSessionData>
{
    /// <inheritdoc />
    public static ICodec<LoginOkSessionData> Codec { get; } = Codecs.Composite(
        Codecs.OptionalByteArray,
        static value => value.CompressedData,
        Codecs.Boolean,
        static value => value.FirstFlag,
        Codecs.Boolean,
        static value => value.SecondFlag,
        static (compressedData, firstFlag, secondFlag) => new LoginOkSessionData(compressedData, firstFlag, secondFlag)
    );
}
