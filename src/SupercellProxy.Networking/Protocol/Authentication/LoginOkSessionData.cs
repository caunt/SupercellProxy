using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>The first LoginOkMessage block native reads only when bytes remain: compressed data and two flags.</summary>
/// <param name="CompressedData">The compressed data.</param>
/// <param name="FirstFlag">The first flag.</param>
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
