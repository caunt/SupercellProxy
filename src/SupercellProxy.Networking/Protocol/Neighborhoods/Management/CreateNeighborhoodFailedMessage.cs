using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Management;

/// <summary>Reports why creating a neighborhood failed.</summary>
/// <param name="Reason">1 to 7; 2 and 5 are profanities in the name and description, 3 a name that is too short and 7 not enough coins.</param>
public sealed record CreateNeighborhoodFailedMessage(int Reason) : CodecMessage<CreateNeighborhoodFailedMessage>, ICodecProvider<CreateNeighborhoodFailedMessage>
{
    /// <inheritdoc />
    public static ICodec<CreateNeighborhoodFailedMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.Reason, static (reason) => new CreateNeighborhoodFailedMessage(reason));
}
