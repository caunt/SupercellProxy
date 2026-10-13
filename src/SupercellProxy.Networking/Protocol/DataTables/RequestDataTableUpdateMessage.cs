using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DataTables;

/// <summary>Requests an update of a data table.</summary>
/// <param name="TableId">The table id.</param>
public sealed record RequestDataTableUpdateMessage(int TableId) : CodecMessage<RequestDataTableUpdateMessage>, ICodecProvider<RequestDataTableUpdateMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDataTableUpdateMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.TableId, static (tableId) => new RequestDataTableUpdateMessage(tableId));
}
