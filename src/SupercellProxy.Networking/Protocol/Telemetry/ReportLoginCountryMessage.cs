using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports the device's countries and languages.</summary>
/// <param name="Data">Encoded country and language details, kept as received; null when none are sent.</param>
public sealed record ReportLoginCountryMessage(ReadOnlyMemory<byte>? Data) : CodecMessage<ReportLoginCountryMessage>, ICodecProvider<ReportLoginCountryMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportLoginCountryMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalByteArray, static message => message.Data, static data => new ReportLoginCountryMessage(data));
}
