using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DataTables;

/// <summary>Requests the data tables.</summary>
public sealed record RequestDataTablesMessage : CodecMessage<RequestDataTablesMessage>, ICodecProvider<RequestDataTablesMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDataTablesMessage> Codec { get; } = Codecs.Unit(static () => new RequestDataTablesMessage());
}
