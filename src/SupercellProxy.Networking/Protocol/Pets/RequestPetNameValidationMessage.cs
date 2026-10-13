using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Asks the server to validate the entered pet name.</summary>
public sealed record RequestPetNameValidationMessage : CodecMessage<RequestPetNameValidationMessage>, ICodecProvider<RequestPetNameValidationMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestPetNameValidationMessage> Codec { get; } = Codecs.Unit(static () => new RequestPetNameValidationMessage());
}
