using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>Collects the postman's token delivery.</summary>
public sealed record CollectPostmanDeliveryCommand : CodecCommand<CollectPostmanDeliveryCommand>, ICommandCodecProvider<CollectPostmanDeliveryCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectPostmanDeliveryCommand> Codec { get; } = Codecs.Command(type: 678, Codecs.Unit(static () => new CollectPostmanDeliveryCommand()));
}
