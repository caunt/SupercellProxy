using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Gallery;

/// <summary>Uploads a decoration design.</summary>
/// <param name="EventId">Decoration event the design is submitted to.</param>
/// <param name="Design">The design's data as the client serialised it for upload.</param>
public sealed record UploadDecorationDesignMessage(int EventId, ReadOnlyMemory<byte>? Design) : CodecMessage<UploadDecorationDesignMessage>, ICodecProvider<UploadDecorationDesignMessage>
{
    /// <inheritdoc />
    public static ICodec<UploadDecorationDesignMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.EventId,
        Codecs.Flagged(Codecs.OptionalByteArray, absent: null, static design => design is not null),
        static message => message.Design,
        static (eventId, design) => new UploadDecorationDesignMessage(eventId, design)
    );
}
