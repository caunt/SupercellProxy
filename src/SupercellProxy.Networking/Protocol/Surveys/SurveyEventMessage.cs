using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Surveys;

/// <summary>Reports a survey answer or action as a JSON payload.</summary>
/// <param name="Payload">The payload.</param>
public sealed record SurveyEventMessage(ReadOnlyMemory<byte>? Payload) : CodecMessage<SurveyEventMessage>, ICodecProvider<SurveyEventMessage>
{
    /// <inheritdoc />
    public static ICodec<SurveyEventMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalByteArray, static message => message.Payload, static (payload) => new SurveyEventMessage(payload));
}
