using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Uploads client debug data.</summary>
/// <param name="Data">The data.</param>
public sealed record UploadClientDebugDataMessage(ReadOnlyMemory<byte>? Data) : CodecMessage<UploadClientDebugDataMessage>, ICodecProvider<UploadClientDebugDataMessage>
{
    /// <inheritdoc />
    public static ICodec<UploadClientDebugDataMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalByteArray, static message => message.Data, static (data) => new UploadClientDebugDataMessage(data));
}
