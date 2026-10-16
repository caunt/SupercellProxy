using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Management;

/// <summary>Reports why changing the neighborhood settings failed.</summary>
/// <param name="Reason">2, 4 and 5 are handled; 2 and 5 are profanities in the name and description.</param>
public sealed record ChangeNeighborhoodSettingsFailedMessage(int Reason) : CodecMessage<ChangeNeighborhoodSettingsFailedMessage>, ICodecProvider<ChangeNeighborhoodSettingsFailedMessage>
{
    /// <inheritdoc />
    public static ICodec<ChangeNeighborhoodSettingsFailedMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.Reason, static reason => new ChangeNeighborhoodSettingsFailedMessage(reason));
}
