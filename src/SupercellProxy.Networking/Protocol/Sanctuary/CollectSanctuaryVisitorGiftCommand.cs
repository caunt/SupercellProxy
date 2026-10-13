using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Collects the sanctuary visitor gift.</summary>
public sealed record CollectSanctuaryVisitorGiftCommand : CodecCommand<CollectSanctuaryVisitorGiftCommand>, ICommandCodecProvider<CollectSanctuaryVisitorGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectSanctuaryVisitorGiftCommand> Codec { get; } = Codecs.Command(type: 566, Codecs.Unit(static () => new CollectSanctuaryVisitorGiftCommand()));
}
