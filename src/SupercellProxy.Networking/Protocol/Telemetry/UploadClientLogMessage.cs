using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Uploads a client log chunk.</summary>
/// <param name="Index">Position of this chunk in the uploaded log.</param>
/// <param name="Data">Raw bytes of the chunk; null when none are sent.</param>
public sealed record UploadClientLogMessage(int Index, ReadOnlyMemory<byte>? Data) : CodecMessage<UploadClientLogMessage>, ICodecProvider<UploadClientLogMessage>
{
    /// <inheritdoc />
    public static ICodec<UploadClientLogMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static message => message.Index,
        Codecs.OptionalByteArray,
        static message => message.Data,
        static (index, data) => new UploadClientLogMessage(index, data)
    );
}
