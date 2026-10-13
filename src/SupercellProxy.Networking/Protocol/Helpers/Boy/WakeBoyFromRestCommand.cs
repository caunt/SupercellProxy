using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers.Boy;

/// <summary>Wakes the boy from his rest.</summary>
public sealed record WakeBoyFromRestCommand : CodecCommand<WakeBoyFromRestCommand>, ICommandCodecProvider<WakeBoyFromRestCommand>
{
    /// <inheritdoc />
    public static CommandCodec<WakeBoyFromRestCommand> Codec { get; } = Codecs.Command(type: 595, Codecs.Unit(static () => new WakeBoyFromRestCommand()));
}
